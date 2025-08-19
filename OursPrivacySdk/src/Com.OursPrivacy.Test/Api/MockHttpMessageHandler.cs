using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Com.OursPrivacy.Test.Api
{
    /// <summary>
    /// A mock HttpMessageHandler for unit testing HttpClient calls.
    /// </summary>
    public class MockHttpMessageHandler : HttpMessageHandler
    {
        public int CallCount { get; private set; } = 0;
        public HttpRequestMessage? LastRequest { get; private set; }
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.OK;
        public string ResponseContent { get; set; } = "{\"success\":true}";

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            CallCount++;
            LastRequest = request;
            var response = new HttpResponseMessage(ResponseStatusCode)
            {
                Content = new StringContent(ResponseContent)
            };
            return Task.FromResult(response);
        }
    }
}
