#

## Generator

Run the following to generate the project

```
openapi-generator-cli generate \
  -i ./swagger.json \
  -g csharp \
  -o ./OursPrivacySdk \
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
