# ApiGift .NET SDK

[![NuGet](https://img.shields.io/nuget/v/ApiGift.Sdk.svg)](https://www.nuget.org/packages/ApiGift.Sdk)
[![Build](https://github.com/ApiGift/ApiGift.DotNetSdk/actions/workflows/build.yml/badge.svg)](https://github.com/ApiGift/ApiGift.DotNetSdk/actions/workflows/build.yml)
[![License](https://img.shields.io/github/license/ApiGift/ApiGift.DotNetSdk.svg)](LICENSE)

Official .NET SDK for ApiGift Shop Gateway v2 APIs.

This SDK is v2-only. It does not reference ApiGift backend contracts,
application layers, domain models, or Entity Framework projects.

GitHub repository: [ApiGift.DotNetSdk](https://github.com/ApiGift/ApiGift.DotNetSdk)

## Installation

```powershell
dotnet add package ApiGift.Sdk --version 0.1.0-preview.1
```

Most applications only need `ApiGift.Sdk`. The `ApiGift.Sdk.Contracts` package
is published separately for advanced scenarios where shared request/response
contracts are useful without the HTTP client implementation.

## Solution structure

- `ApiGift.Sdk.Contracts` - SDK-owned v2 request and response contracts
- `ApiGift.Sdk` - root client, typed feature clients, authentication, transport,
  and dependency-injection registration
- `ApiGift.Sdk.Tests` - unit tests and opt-in integration tests
- `samples/ConsoleSample` - environment-variable-based console application
- `ApiGift.DotNetSdk.sln` - SDK solution
- `SDK-GENERATION.md` - endpoint and contract maintenance process
- `V2-ENDPOINT-COVERAGE.md` - controller-to-client endpoint coverage
- `RELEASE-NOTES.md` - release history

## Dependency-injection usage

```csharp
using ApiGift.Sdk.V2;

services.AddApiGiftSdk(options =>
{
    options.BaseUrl = "https://api.apigift.com/";
    options.AccessKey = configuration["ApiGift:AccessKey"]!;
    options.SecretKey = configuration["ApiGift:SecretKey"]!;
});
```

Resolve or inject `IApiGiftClient`:

```csharp
public sealed class ProductService(IApiGiftClient apiGiftClient)
{
    public Task<IReadOnlyList<ApiGift.Sdk.Contracts.V2.Products.ProductSummary>>
        GetProductsAsync(CancellationToken cancellationToken)
    {
        return apiGiftClient.Products.GetProductsAsync(
            cancellationToken: cancellationToken);
    }
}
```

Direct construction remains supported:

```csharp
IApiGiftClient client = new ApiGiftClient(
    new HttpClient(),
    new ApiGiftClientOptions
    {
        BaseAddress = new Uri("https://api.apigift.com/"),
        AccessKey = "your-access-key",
        SecretKey = "your-secret-key"
    });
```

## Authentication

The default `AccessKeySecretKeyAuthenticationProvider` applies:

```text
access-key: {AccessKey}
secret-key: {SecretKey}
```

Authentication is abstracted behind `IApiGiftAuthenticationProvider`. Register a
custom implementation before `AddApiGiftSdk` to replace the default provider.

Never commit access keys or secret keys.

## Exception handling

Non-success responses are mapped to:

- HTTP 400 - `ApiGiftValidationException`
- HTTP 401 - `ApiGiftUnauthorizedException`
- HTTP 403 - `ApiGiftForbiddenException`
- HTTP 404 - `ApiGiftNotFoundException`
- HTTP 429 - `ApiGiftRateLimitException`
- Other non-success statuses - `ApiGiftException`

Exceptions include `StatusCode`, `ResponseBody`, and `RequestPath`.
Documented subscription 400/409 response bodies remain typed API results where
the endpoint contract defines them that way.

## SDK version headers

Every request includes:

```text
User-Agent: ApiGift.DotNetSdk/{version}
X-SDK-Version: {version}
```

The version is read from the SDK assembly informational version, falling back to
the assembly version.

## Console sample

Configure credentials through environment variables:

```powershell
$env:APIGIFT_BASE_URL = "https://api.apigift.com/"
$env:APIGIFT_ACCESS_KEY = "your-access-key"
$env:APIGIFT_SECRET_KEY = "your-secret-key"
dotnet run --project samples\ConsoleSample\ConsoleSample.csproj
```

The sample is read-only. It demonstrates wallet balance, catalog, product
availability, and status lookup endpoints. It does not create, cancel, return,
pay, or otherwise mutate ApiGift resources.

## Integration tests

Integration tests are skipped by default. They call only a read-only v2
endpoint. To enable them, set:

```powershell
$env:APIGIFT_RUN_INTEGRATION_TESTS = "true"
$env:APIGIFT_BASE_URL = "https://api.apigift.com/"
$env:APIGIFT_ACCESS_KEY = "your-access-key"
$env:APIGIFT_SECRET_KEY = "your-secret-key"
```

Run only integration tests with:

```powershell
dotnet test ApiGift.DotNetSdk.sln --filter "Category=Integration"
```

## Build, test, and package

From the `SDKs` directory:

```powershell
dotnet restore ApiGift.DotNetSdk.sln
dotnet build ApiGift.DotNetSdk.sln
dotnet test ApiGift.DotNetSdk.sln
dotnet pack ApiGift.Sdk\ApiGift.Sdk.csproj -c Release
```

See `SDK-GENERATION.md` before adding or regenerating endpoints.

## NuGet publication readiness

- README, XML documentation, and symbol packages are generated.
- NuGet package validation is enabled for both published projects.
- The package repository is `ApiGift/ApiGift.DotNetSdk`.
- Packages include the MIT license file.
- Publish `ApiGift.Sdk.Contracts` before `ApiGift.Sdk` because the main package
  depends on the matching contracts package version.
- A package icon is intentionally deferred until an approved ApiGift brand asset
  is available. Add it through `PackageIcon` and pack the image at the package
  root; do not use an unapproved or remote-only icon.
- Repository metadata is emitted in the NuGet packages.
