using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Com.OursPrivacy.Api;
using Com.OursPrivacy.Model;

public class IndexModel : PageModel
{
    private readonly IOursPrivacyApi _api;
    private readonly ILogger<IndexModel> _logger; // Add logger

    public IndexModel(IOursPrivacyApi api, ILogger<IndexModel> logger)
    {
        _api = api;
        _logger = logger;
    }

    public async Task<IActionResult> OnPostIdentifyAsync(string userId, string? email)
    {
        _logger.LogInformation("Received Identify POST: userId={UserId}, email={Email}", userId, email);

        var userProps = new IdentifyRequestUserProperties();
        if (!string.IsNullOrEmpty(email))
            userProps.Email = email;

        var identifyRequest = new IdentifyRequest(userId, userProps);
        _api.EnqueueIdentify(identifyRequest);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostTrackAsync(string userId, string eventName)
    {
        _logger.LogInformation("Received Track POST: userId={UserId}, eventName={EventName}", userId, eventName);

        var trackRequest = new TrackRequest(userId: userId ?? "anonymous", @event: eventName, 
            eventProperties: new Dictionary<string, object>
            {
                { "deviceId", "fa197c5a-de88-471e-bd12-5f1f9c7a7a42" },
                { "userId", "fa197c5a-de88-471e-bd12-5f1f9c7a7a42" },
                { "testInt", 1 }
            });
        _api.EnqueueTrack(trackRequest);
        return RedirectToPage();
    }
}