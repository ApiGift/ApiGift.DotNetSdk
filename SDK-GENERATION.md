# SDK Generation Guide

Repository: [ApiGift.DotNetSdk](https://github.com/ApiGift/ApiGift.DotNetSdk)

## Source of truth

The only endpoint-discovery source for this SDK is the public Shop Gateway v2
controller set in the ApiGift source repository:

`ApiGift.WebAPI/Controllers/ShopGateway/v2`

The SDK targets only ApiGift Shop Gateway v2. Do not add v1 routes, compatibility
clients, or v1 contracts.

## Contract boundaries

SDK contracts are derived from:

- Public controller actions and route metadata
- Controller request parameter types
- Swagger response metadata
- Public JSON request and response shapes
- Swagger descriptions where they clarify optionality or behavior

The SDK must not reference:

- backend API contract projects
- server-side shared contract assemblies
- Application commands or queries
- Internal domain models
- Entity Framework entities

Equivalent SDK-owned DTOs belong in `ApiGift.Sdk.Contracts` under the relevant
v2 feature namespace.

## Adding a new endpoint

1. Inspect the new public action under `Controllers/ShopGateway/v2`.
2. Confirm the final HTTP method, route, query parameters, body, response type,
   status codes, and Swagger visibility.
3. Add or update SDK-owned contracts without referencing server assemblies.
4. Add the method to the appropriate client interface.
5. Implement the method using the shared `ApiGiftHttpClient`.
6. Include `CancellationToken cancellationToken = default` and pass it through.
7. Update `V2-ENDPOINT-COVERAGE.md`.
8. Add transport or contract tests when the endpoint introduces new behavior.
9. Run restore, build, and tests for `ApiGift.DotNetSdk.sln`.

Do not place serialization, authentication, status handling, or raw
`HttpClient.SendAsync` logic in feature clients.

## Manual generation and future automation

The current SDK is maintained manually because the controllers and Swagger
metadata are the authoritative public surface while server-side DTOs include
internal dependencies that must not leak into the SDK.

A future Swagger/NSwag pipeline can generate transport contracts and client
methods if it:

- Consumes only the Shop Gateway v2 Swagger document
- Preserves the current public client grouping
- Generates SDK-owned contracts
- Uses the shared authentication and HTTP pipeline
- Excludes `[SwaggerIgnore]` actions
- Produces deterministic output that can be reviewed before replacement

Until such a pipeline exists, controller-to-SDK coverage must be reviewed and
recorded manually.

No paginated Shop Gateway v2 response contracts currently exist. If pagination
is introduced, use one shared `PagedResponse<T>` contract rather than creating
endpoint-specific pagination wrappers.
