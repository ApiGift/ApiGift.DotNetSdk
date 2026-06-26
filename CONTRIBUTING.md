# Contributing

Thank you for contributing to the ApiGift .NET SDK.

## Development

Use the SDK solution from the repository root:

```powershell
dotnet restore ApiGift.DotNetSdk.sln
dotnet build ApiGift.DotNetSdk.sln -c Release
dotnet test ApiGift.DotNetSdk.sln -c Release
```

## Guidelines

- Keep the SDK v2-only.
- Do not reference ApiGift server-side projects or internal contracts.
- Keep public API changes intentional, documented, and covered by tests.
- Do not commit API keys, secrets, local paths, or customer data.

## Pull requests

Open a pull request with a clear summary, test results, and any public API
impact. Integration tests must remain opt-in and safe by default.
