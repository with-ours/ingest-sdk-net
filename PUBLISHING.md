# Publishing Guide - .NET SDK

This document describes how to build, version, and publish the OursPrivacy .NET SDK to NuGet.

## Overview

- **Package Name**: `Com.OursPrivacy`
- **Target Registry**: NuGet (https://www.nuget.org/)
- **Current Status**: Not yet published to NuGet
- **Build System**: AppVeyor CI + Manual NuGet publishing

## Prerequisites

### Tools Required
- .NET 8.0 SDK or later
- NuGet CLI or `dotnet` CLI
- Access to NuGet.org account with publishing permissions

### Authentication Setup
1. Create/obtain NuGet API key from https://www.nuget.org/account/apikeys
2. Store API key securely (do not commit to repository)
3. Configure API key locally:
   ```bash
   dotnet nuget push --help  # To see configuration options
   ```

## Version Management

### Current Version
The current version is defined in `src/Com.OursPrivacy/Com.OursPrivacy.csproj`:
```xml
<Version>1.0.0</Version>
```

### Updating Version
1. Edit `src/Com.OursPrivacy/Com.OursPrivacy.csproj`
2. Update the `<Version>` element:
   ```xml
   <Version>1.0.1</Version>
   ```
3. Update release notes in the `<PackageReleaseNotes>` element
4. Commit changes with descriptive message

## Building the Package

### Local Build
```bash
# Clean previous builds
dotnet clean

# Restore dependencies
dotnet restore

# Build in Release mode
dotnet build -c Release

# Create NuGet package
dotnet pack src/Com.OursPrivacy/Com.OursPrivacy.csproj -c Release -o ./packages
```

### AppVeyor Build
The repository includes `appveyor.yml` that automatically:
1. Builds the project in Release mode
2. Runs tests
3. Creates NuGet package in `./output` directory

The AppVeyor build is triggered on every commit.

## Publishing Process

### Manual Publishing (Initial Setup)

1. **Build the package locally**:
   ```bash
   dotnet pack src/Com.OursPrivacy/Com.OursPrivacy.csproj -c Release -o ./packages
   ```

2. **Verify package contents**:
   ```bash
   # List contents of the .nupkg file
   unzip -l ./packages/Com.OursPrivacy.1.0.0.nupkg
   ```

3. **Test package locally** (optional):
   ```bash
   # Install to a test project
   dotnet add package Com.OursPrivacy --source ./packages
   ```

4. **Publish to NuGet**:
   ```bash
   dotnet nuget push ./packages/Com.OursPrivacy.1.0.0.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
   ```

### Automated Publishing (Future Setup)

To set up automated publishing via AppVeyor:

1. **Add NuGet API key to AppVeyor**:
   - Go to AppVeyor project settings
   - Add secure environment variable `NUGET_API_KEY`

2. **Update `appveyor.yml`**:
   ```yaml
   deploy:
     provider: NuGet
     api_key:
       secure: YOUR_ENCRYPTED_API_KEY
     on:
       branch: main
       appveyor_repo_tag: true
   ```

## Release Workflow

### Standard Release Process

1. **Update version and release notes**:
   ```bash
   # Edit src/Com.OursPrivacy/Com.OursPrivacy.csproj
   # Update <Version> and <PackageReleaseNotes>
   ```

2. **Commit and tag**:
   ```bash
   git add src/Com.OursPrivacy/Com.OursPrivacy.csproj
   git commit -m "chore: release v1.0.1"
   git tag -a v1.0.1 -m "Release v1.0.1 - Description of changes"
   git push origin main
   git push origin v1.0.1
   ```

3. **Build and publish**:
   ```bash
   dotnet pack src/Com.OursPrivacy/Com.OursPrivacy.csproj -c Release -o ./packages
   dotnet nuget push ./packages/Com.OursPrivacy.1.0.1.nupkg --api-key YOUR_API_KEY --source https://api.nuget.org/v3/index.json
   ```

4. **Verify publication**:
   - Check https://www.nuget.org/packages/Com.OursPrivacy/
   - Test installation: `dotnet add package Com.OursPrivacy`

## Package Configuration

### Key Package Properties
The package is configured in `Com.OursPrivacy.csproj` with these important settings:

```xml
<PackageId>Com.OursPrivacy</PackageId>
<Version>1.0.0</Version>
<Authors>OpenAPI</Authors>
<Description>A library generated from a OpenAPI doc</Description>
<PackageReleaseNotes>Minor update</PackageReleaseNotes>
<RepositoryUrl>https://github.com/GIT_USER_ID/GIT_REPO_ID.git</RepositoryUrl>
```

### Recommended Updates
Before publishing, consider updating:
- `<Authors>` to "OursPrivacy Team" or "Ours Privacy, Inc"
- `<Description>` to more descriptive text
- `<RepositoryUrl>` to actual GitHub URL
- `<PackageReleaseNotes>` for each release

## Code Generation

This SDK is auto-generated from OpenAPI specifications. To regenerate:

```bash
openapi-generator-cli generate \
  -i ./swagger.json \
  -g csharp \
  -o . \
  --additional-properties=packageName=Com.OursPrivacy,packageGuid=fe562c1d-d0e1-4c13-b051-ddf3707fb8a2,netCoreProject=true,targetFramework=net8.0,generatePackageOnBuild=true,useSystemTextJson=true,generateInterfaces=true
```

**Important**: After regeneration, verify that package settings in the `.csproj` file are correct before publishing.

## Troubleshooting

### Common Issues

1. **"Package already exists"**: Increment version number
2. **"API key invalid"**: Check API key permissions and expiration
3. **"Package validation failed"**: Review package metadata and dependencies

### Verification Steps
```bash
# Check package info
dotnet nuget locals all --list

# Test package installation
dotnet new console -n TestApp
cd TestApp
dotnet add package Com.OursPrivacy
dotnet build
```

## Security Notes

- Never commit API keys to the repository
- Use environment variables or secure CI/CD secrets
- Regularly rotate API keys
- Review package dependencies for security vulnerabilities

## Support

- **Documentation**: https://docs.oursprivacy.com/docs/dotnet#/
- **Issues**: https://github.com/with-ours/ingest-sdk-net/issues
- **NuGet Package**: https://www.nuget.org/packages/Com.OursPrivacy/ (once published)