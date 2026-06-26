using ApiGift.Sdk.Contracts.V2.Orders;

namespace ApiGift.Sdk.V2.Orders;

/// <summary>Provides subscription activation and status operations.</summary>
public interface IOrdersClient
{
    /// <summary>Creates an idempotent subscription activation.</summary>
    Task<CreateSubscriptionActivationResponse> CreateSubscriptionActivationAsync(
        CreateSubscriptionActivationRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Validates subscription identity values.</summary>
    Task<ValidateSubscriptionIdentifierResponse> ValidateSubscriptionIdentifierAsync(
        ValidateSubscriptionIdentifierRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Gets subscription order status by reference identifier.</summary>
    Task<SubscriptionStatus> GetSubscriptionStatusAsync(
        string referenceId,
        CancellationToken cancellationToken = default);

}
