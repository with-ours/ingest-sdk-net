# Com.OursPrivacy .NET SDK

The Ours Privacy Server-Side REST API SDK for .NET provides a comprehensive client library for integrating with the Ours privacy platform.

## Installation

### Prerequisites
- .NET 8.0 or later
- Visual Studio 2022 or equivalent development environment

### Install the Package

```bash
# Package Manager Console
Install-Package Com.OursPrivacy

# .NET CLI
dotnet add package Com.OursPrivacy
```

> **Note**: This package is not yet published to NuGet. See [PUBLISHING.md](PUBLISHING.md) for information on how to publish it.

## Quick Start

1. Install the package
2. Configure your API key in your application settings
3. Register the services in your DI container
4. Start making API calls to identify users and track events

## Usage

### Registering Your API Key

Before making API calls, register your API key(s) using the provided DI helpers.  
**Recommended:** Load your API key from configuration.

```csharp
// In your Program.cs or Startup.cs
var apiKey = builder.Configuration["OursPrivacy:ApiKey"] 
    ?? throw new InvalidOperationException("OursPrivacy:ApiKey is not configured.");

builder.Host.ConfigureOursPrivacy((context, services, options) =>
{
    options.ConfigureApiKey(apiKey);
    // ...other configuration...
});
```

---

### Configuring Batching

Configure batch size and timer. Batches will send when the batch size or timer is reached.

Note: `EnqueueIdentify` and `EnqueueTrack` must be used to utilize batching.

```csharp
options.AddEventBatch(
    batchSize: 2, // Number of events before sending
    maxWaitTime: TimeSpan.FromSeconds(10) // Max time to wait before sending
);
```

---

### Configuring HttpClient Policies

You can add Polly policies and other middleware to the API HttpClient:

```csharp
options.AddApiHttpClients(client =>
{
    client.BaseAddress = new Uri("https://dev-api.oursprivacy.com/");
}, builder =>
{
    builder
        .AddRetryPolicy(2)
        .AddTimeoutPolicy(TimeSpan.FromSeconds(5))
        .AddCircuitBreakerPolicy(10, TimeSpan.FromSeconds(30));
});
```

---

### Using the library in your project

```csharp
using Com.OursPrivacy.Api;
using Com.OursPrivacy.Client;
using Com.OursPrivacy.Model;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Load API key from configuration
var apiKey = builder.Configuration["OursPrivacy:ApiKey"] 
    ?? throw new InvalidOperationException("OursPrivacy:ApiKey is not configured.");

// Register OursPrivacyApi dependencies
builder.Host.ConfigureOursPrivacy((context, services, options) =>
{
    options.ConfigureApiKey(apiKey);
    options.AddApiHttpClients(client =>
    {
      // HttpClient configurations here
    }, builder =>
    {
        builder
            .AddRetryPolicy(2)
            .AddTimeoutPolicy(TimeSpan.FromSeconds(5))
            .AddCircuitBreakerPolicy(10, TimeSpan.FromSeconds(30));
    });
    options.AddEventBatch(2, TimeSpan.FromSeconds(10));
});

var app = builder.Build();

var api = app.Services.GetRequiredService<IOursPrivacyApi>();

// Example: Identify request with queue and batching
var identifyRequest = new IdentifyRequest(
    userId: "user-123",
    userProperties: new IdentifyRequestUserProperties { }
);

// Queues the request for batching
api.EnqueueIdentify(identifyRequest);

// Sends the request immediately without a queue or batching
IIdentifyApiResponse apiResponse = await api.IdentifyAsync(identifyRequest);
```

---

## Questions

- **What about HttpRequest failures and retries?**  
  Configure Polly in the IHttpClientBuilder as shown above.

- **Does an HttpRequest throw an error when the server response is not Ok?**  
  If the return type is `ApiResponse<T>`, no error will be thrown; check the `StatusCode` and `ReasonPhrase`.  
  If the return type is `T`, an exception will be thrown for non-success responses.

- **How do I validate requests and process responses?**  
  Use the provided `On` and `After` partial methods in the API classes.

---

## Development

### Regenerating the SDK

Run the following to regenerate the SDK from the OpenAPI specification:

```bash
openapi-generator-cli generate \
  -i ./swagger.json \
  -g csharp \
  -o . \
  --additional-properties=packageName=Com.OursPrivacy,packageGuid=fe562c1d-d0e1-4c13-b051-ddf3707fb8a2,netCoreProject=true,targetFramework=net8.0,generatePackageOnBuild=true,useSystemTextJson=true,generateInterfaces=true
```

| Option                         | Description                                                                 |
|-------------------------------|-----------------------------------------------------------------------------|
| `packageName`                 | Sets the root namespace and NuGet package name.                            |
| `packageGuid`                 | Unique identifier for the NuGet package. Required for packaging.           |
| `netCoreProject`              | Uses modern SDK-style `.csproj` format.                                     |
| `targetFramework`             | Specifies the .NET target framework (e.g., `net8.0`).                       |
| `generatePackageOnBuild`      | Builds a `.nupkg` automatically when running `dotnet build`.               |
| `useSystemTextJson`           | Uses `System.Text.Json` instead of `Newtonsoft.Json` for serialization.     |
| `generateInterfaces`          | Adds interfaces for API clients to support mocking and unit testing.       |

### Build Information

- SDK version: 1.0.0
- Generator version: 7.14.0
- Build package: org.openapitools.codegen.languages.CSharpClientCodegen
- API: Ours Server-Side REST API v1.0.0

This C# SDK is automatically generated by the [OpenAPI Generator](https://openapi-generator.tech) project.

## API Reference

For detailed API documentation, see:
- [API Endpoints](docs/apis/OursPrivacyApi.md) - Complete endpoint documentation
- [Data Models](docs/models/) - Request and response model documentation
  - [IdentifyRequest](docs/models/IdentifyRequest.md)
  - [TrackRequest](docs/models/TrackRequest.md)
  - [Track200Response](docs/models/Track200Response.md)
- [Generated Client README](src/Com.OursPrivacy/README.md) - Additional configuration options

## Contributing

For information on how to contribute to this project, including how to build and publish releases, see [PUBLISHING.md](PUBLISHING.md).

## Support

- **Documentation**: https://docs.oursprivacy.com/docs/dotnet#/
- **NuGet Package**: https://www.nuget.org/packages/Com.OursPrivacy/ (once published)
- **Issues**: https://github.com/with-ours/ingest-sdk-net/issues
- **Repository**: https://github.com/with-ours/ingest-sdk-net
