using ApiGift.Sdk.Contracts.V2.Orders;
using ApiGift.Sdk.V2;
using ApiGift.Sdk.V2.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Text;
using Xunit;

namespace ApiGift.Sdk.Tests;

public sealed class ApiGiftClientTests
{
    [Fact]
    public async Task RequestIncludesAuthenticationAndVersionHeaders()
    {
        RecordingHandler handler = new(
            HttpStatusCode.OK,
            """{"balance":42}""");
        ApiGiftClient client = CreateClient(handler);

        await client.Wallet.GetOrderCreditAsync();

        Assert.Equal("access", handler.GetHeader("access-key"));
        Assert.Equal("secret", handler.GetHeader("secret-key"));
        Assert.StartsWith(
            "ApiGift.DotNetSdk/",
            handler.GetHeader("User-Agent"),
            StringComparison.Ordinal);
        Assert.False(string.IsNullOrWhiteSpace(handler.GetHeader("X-SDK-Version")));
    }

    [Theory]
    [InlineData(HttpStatusCode.BadRequest, typeof(ApiGiftValidationException))]
    [InlineData(HttpStatusCode.Unauthorized, typeof(ApiGiftUnauthorizedException))]
    [InlineData(HttpStatusCode.Forbidden, typeof(ApiGiftForbiddenException))]
    [InlineData(HttpStatusCode.NotFound, typeof(ApiGiftNotFoundException))]
    [InlineData(HttpStatusCode.TooManyRequests, typeof(ApiGiftRateLimitException))]
    [InlineData(HttpStatusCode.InternalServerError, typeof(ApiGiftException))]
    public async Task NonSuccessResponsesMapToSdkExceptions(
        HttpStatusCode statusCode,
        Type expectedExceptionType)
    {
        RecordingHandler handler = new(statusCode, """{"message":"failed"}""");
        ApiGiftClient client = CreateClient(handler);

        ApiGiftException exception = await Assert.ThrowsAsync(
            expectedExceptionType,
            () => client.Wallet.GetOrderCreditAsync()) as ApiGiftException
            ?? throw new InvalidOperationException("Expected ApiGiftException.");

        Assert.Equal(statusCode, exception.StatusCode);
        Assert.Equal("""{"message":"failed"}""", exception.ResponseBody);
        Assert.Equal("/v2/OrderCredit", exception.RequestPath);
    }

    [Fact]
    public async Task DocumentedSubscriptionValidationResponseIsDeserialized()
    {
        RecordingHandler handler = new(
            HttpStatusCode.BadRequest,
            """{"status":400,"message":"invalid product"}""");
        ApiGiftClient client = CreateClient(handler);

        CreateSubscriptionActivationResponse response =
            await client.Orders.CreateSubscriptionActivationAsync(
                new CreateSubscriptionActivationRequest
                {
                    ProductId = Guid.NewGuid(),
                    ReferenceId = "reference",
                    Identities = []
                });

        Assert.Equal(400, response.Status);
        Assert.Equal("invalid product", response.Message);
    }

    [Fact]
    public async Task CancellationTokenIsPassedToHttpPipeline()
    {
        CancellationHandler handler = new();
        ApiGiftClient client = CreateClient(handler);
        using CancellationTokenSource cancellation = new();
        cancellation.Cancel();

        await Assert.ThrowsAnyAsync<OperationCanceledException>(
            () => client.Wallet.GetOrderCreditAsync(cancellation.Token));
    }

    [Fact]
    public async Task DependencyInjectionUsesRegisteredAuthenticationProvider()
    {
        RecordingAuthenticationProvider authenticationProvider = new();
        ServiceCollection services = new();
        services.AddSingleton<IApiGiftAuthenticationProvider>(
            authenticationProvider);
        services.AddApiGiftSdk(options =>
        {
            options.BaseUrl = "https://example.test/";
            options.AccessKey = "unused-access";
            options.SecretKey = "unused-secret";
        });

        await using ServiceProvider provider = services.BuildServiceProvider();
        IApiGiftAuthenticationProvider resolvedProvider =
            provider.GetRequiredService<IApiGiftAuthenticationProvider>();
        using HttpRequestMessage request = new();

        await resolvedProvider.ApplyAuthenticationAsync(request);

        Assert.Same(authenticationProvider, resolvedProvider);
        Assert.Equal("custom", request.Headers.GetValues("custom-auth").Single());
    }

    private static ApiGiftClient CreateClient(HttpMessageHandler handler)
    {
        return new ApiGiftClient(
            new HttpClient(handler),
            new ApiGiftClientOptions
            {
                BaseUrl = "https://example.test/",
                AccessKey = "access",
                SecretKey = "secret"
            });
    }

    private sealed class RecordingHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode statusCode;
        private readonly string responseBody;
        private Dictionary<string, string[]> headers = [];

        public RecordingHandler(HttpStatusCode statusCode, string responseBody)
        {
            this.statusCode = statusCode;
            this.responseBody = responseBody;
        }

        public string GetHeader(string name)
        {
            return headers.TryGetValue(name, out string[]? values)
                ? string.Join(" ", values)
                : string.Empty;
        }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            headers = request.Headers.ToDictionary(
                header => header.Key,
                header => header.Value.ToArray(),
                StringComparer.OrdinalIgnoreCase);

            return Task.FromResult(new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(
                    responseBody,
                    Encoding.UTF8,
                    "application/json")
            });
        }
    }

    private sealed class CancellationHandler : HttpMessageHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            throw new InvalidOperationException("Unreachable.");
        }
    }

    private sealed class RecordingAuthenticationProvider
        : IApiGiftAuthenticationProvider
    {
        public ValueTask ApplyAuthenticationAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken = default)
        {
            request.Headers.Add("custom-auth", "custom");
            return ValueTask.CompletedTask;
        }
    }
}
