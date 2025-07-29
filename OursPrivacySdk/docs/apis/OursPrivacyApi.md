# Com.OursPrivacy.Api.OursPrivacyApi

All URIs are relative to *https://api.oursprivacy.com/api/v1*

| Method | HTTP request | Description |
|--------|--------------|-------------|
| [**Identify**](OursPrivacyApi.md#identify) | **POST** /identify | Identify Users |
| [**Track**](OursPrivacyApi.md#track) | **POST** /track | Track Events |

<a id="identify"></a>
# **Identify**
> Track200Response Identify (IdentifyRequest identifyRequest)

Identify Users

Add user properties to an existing user's profile.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **identifyRequest** | [**IdentifyRequest**](IdentifyRequest.md) | The payload to identify a user |  |

### Return type

[**Track200Response**](Track200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success - your event was sent to our servers |  -  |
| **400** | Bad Request - Something about the body of your request is invalid. Please update your payload and try again. |  -  |
| **401** | Unauthorized - you are not authorized to send events to Ours. Please contact support. |  -  |
| **429** | Too Many Requests - We recommend starting with a backoff of 2s and doubling backoff until 60s, with 1-5s of jitter. |  -  |
| **500** | Internal Server Error - in the unlikely event that you see this error, please backoff as described for a 429 response. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

<a id="track"></a>
# **Track**
> Track200Response Track (TrackRequest trackRequest)

Track Events

Track events from your server. Please include at least one of: userId, externalId, or email. These properties help us associate events with existing users. For all fields, null values unset the property and undefined values do not unset existing properties.


### Parameters

| Name | Type | Description | Notes |
|------|------|-------------|-------|
| **trackRequest** | [**TrackRequest**](TrackRequest.md) | The payload to track an event |  |

### Return type

[**Track200Response**](Track200Response.md)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
| **200** | Success - your event was sent to our servers |  -  |
| **400** | Bad Request - Something about the body of your request is invalid. Please update your payload and try again. |  -  |
| **401** | Unauthorized - you are not authorized to send events to Ours. Please contact support. |  -  |
| **429** | Too Many Requests - We recommend starting with a backoff of 2s and doubling backoff until 60s, with 1-5s of jitter. |  -  |
| **500** | Internal Server Error - in the unlikely event that you see this error, please backoff as described for a 429 response. |  -  |

[[Back to top]](#) [[Back to API list]](../../README.md#documentation-for-api-endpoints) [[Back to Model list]](../../README.md#documentation-for-models) [[Back to README]](../../README.md)

