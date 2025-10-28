
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Com.OursPrivacy.Api;
using Com.OursPrivacy.Model;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Com.OursPrivacy.Client
{

    /// <summary>
    /// A class that batches events for processing.
    /// It uses a queue to store events and processes them in batches.
    /// </summary>
    public class EventBatch
    {
        // Batching and queueing
        private readonly ConcurrentQueue<object> _eventQueue = new ConcurrentQueue<object>();
        private readonly object _batchLock = new object();
        private int _batchSize = 10;
        private TimeSpan _maxWaitTime = TimeSpan.FromSeconds(30);
        private System.Timers.Timer _batchTimer;
        private IServiceProvider _serviceProvider;
        private ILogger<EventBatch> _logger;

        /// <summary>
        /// Initializes a new instance of the EventBatch class.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="batchSize"></param>
        /// <param name="maxWaitTime"></param>
        public EventBatch(IServiceProvider serviceProvider, int batchSize, TimeSpan maxWaitTime)
        {
            _batchSize = batchSize;
            _maxWaitTime = maxWaitTime;
            _batchTimer = new System.Timers.Timer(_maxWaitTime.TotalMilliseconds);
            _batchTimer.Elapsed += (s, e) => Flush();
            _batchTimer.AutoReset = false;
            _serviceProvider = serviceProvider;
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            _logger = loggerFactory.CreateLogger<EventBatch>();
        }

        /// <summary>
        /// Processes an event asynchronously.
        /// </summary>
        /// <param name="api">The OursPrivacyApi instance</param>
        /// <param name="evt"></param>
        /// <returns></returns>
        protected async Task ProcessEventAsync(IOursPrivacyApi api, object evt)
        {
            if (evt is IdentifyRequest req)
                await api.IdentifyAsync(req, default);
            else if (evt is TrackRequest treq)
                await api.TrackAsync(treq, default);
        }

        /// <summary>
        /// Enqueue an event for batching. Triggers send if batch size is reached.
        /// </summary>
        /// <param name="evt">The event to enqueue</param>
        public void EnqueueEvent(object evt)
        {
            _eventQueue.Enqueue(evt);
            if (_eventQueue.Count >= _batchSize)
            {
                Flush();
            }
            else
            {
                // Start or reset the timer
                lock (_batchLock)
                {
                    _batchTimer.Stop();
                    _batchTimer.Start();
                }
            }
        }

        /// <summary>
        /// Flushes the event queue, sending all events one at a time.
        /// </summary>
        public async Task FlushAsync()
        {
            await ProcessQueueAsync();
        }

        /// <summary>
        /// Flushes the event queue, sending all events one at a time.
        /// </summary>
        public void Flush()
        {
            Task.Run(async () =>
            {
                try
                {
                    await FlushAsync();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error flushing event batch");
                }
            });
        }

        /// <summary>
        /// Processes the event queue, sending events one at a time.
        /// </summary>
        private async Task ProcessQueueAsync()
        {
            var batch = new List<object>();
            while (batch.Count < _batchSize && _eventQueue.TryDequeue(out var evt))
            {
                if (evt != null) batch.Add(evt);
            }
            if (batch.Count == 0) return;

            var api = _serviceProvider.GetRequiredService<IOursPrivacyApi>();
            foreach (var evt in batch)
            {
                try
                {
                    await ProcessEventAsync(api, evt);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing event: {Event}", evt);
                }
            }
        }
    }
}