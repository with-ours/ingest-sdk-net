# Com.OursPrivacy.Model.TrackRequest

## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**Event** | **string** | The name of the event you&#39;re tracking. This must be whitelisted in the Ours dashboard. | 
**Token** | **string** | The token for your Ours Privacy Source. You can find this in the Ours dashboard. | 
**Time** | **decimal** | The time at which the event occurred, in seconds or milliseconds since UTC epoch. The time must be in the past and within the last 7 days. | [optional] 
**UserId** | **string** | The Ours user id stored in local storage and cookies on your web properties. If userId is included in the request, we do not lookup the user by email or externalId. | [optional] 
**ExternalId** | **string** | The externalId (the ID in your system) of a user. We will associate this event with the user or create a user. If included in the request, email lookup is ignored. | [optional] 
**Email** | **string** | The email address of a user. We will associate this event with the user or create a user. Used for lookup if externalId and userId are not included in the request. | [optional] 
**EventProperties** | **Dictionary&lt;string, Object&gt;** | Any additional event properties you want to pass along. | [optional] 
**UserProperties** | [**TrackRequestUserProperties**](TrackRequestUserProperties.md) |  | [optional] 
**DefaultProperties** | [**TrackRequestDefaultProperties**](TrackRequestDefaultProperties.md) |  | [optional] 
**DistinctId** | **string** | A unique identifier for the event. This helps prevent duplicate events. | [optional] 

[[Back to Model list]](../../README.md#documentation-for-models) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to README]](../../README.md)

