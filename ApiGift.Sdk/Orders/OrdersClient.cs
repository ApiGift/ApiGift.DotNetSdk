using ApiGift.Sdk.Contracts.V2.Orders;
using System.Net;

namespace ApiGift.Sdk.V2.Orders;

/// <inheritdoc />
public sealed class OrdersClient : IOrdersClient
{
    private readonly ApiGiftHttpClient httpClient;

    internal OrdersClient(ApiGiftHttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <inheritdoc />
    public Task<CreateSubscriptionActivationResponse> CreateSubscriptionActivationAsync(
        CreateSubscriptionActivationRequest request,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<CreateSubscriptionActivationResponse>(
            HttpMethod.Post,
            "v2/Subscriptions",
            request,
            cancellationToken,
            [HttpStatusCode.BadRequest, HttpStatusCode.Conflict]);
    }

    /// <inheritdoc />
    public Task<ValidateSubscriptionIdentifierResponse> ValidateSubscriptionIdentifierAsync(
        ValidateSubscriptionIdentifierRequest request,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<ValidateSubscriptionIdentifierResponse>(
            HttpMethod.Post,
            "v2/Subscriptions/validate-identifier",
            request,
            cancellationToken,
            [HttpStatusCode.BadRequest]);
    }

    /// <inheritdoc />
    public Task<SubscriptionStatus> GetSubscriptionStatusAsync(
        string referenceId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(referenceId);

        return httpClient.SendAsync<SubscriptionStatus>(
            HttpMethod.Get,
            $"v2/Subscriptions?referenceId={Uri.EscapeDataString(referenceId)}",
            null,
            cancellationToken);
    }

}
