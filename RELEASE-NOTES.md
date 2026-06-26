# Release Notes

Repository: [ApiGift.DotNetSdk](https://github.com/ApiGift/ApiGift.DotNetSdk)

## 0.1.0-preview.1

Initial preview release of the official ApiGift .NET SDK:

- Shop Gateway v2-only API surface
- Access-key and secret-key authentication
- Centralized HTTP request and response handling
- Typed category, product, inventory, order, merchant, and wallet clients
- SDK-owned public contracts with no backend project dependencies
- Typed exceptions for validation, authentication, authorization, not-found,
  rate-limit, and general API failures
- Dependency-injection registration through `AddApiGiftSdk`
- `User-Agent` and `X-SDK-Version` headers on every request
- Initial unit and opt-in read-only integration test coverage
