using ApiGift.Sdk.Contracts.V2.Wallet;

namespace ApiGift.Sdk.V2.Wallet;

/// <summary>Provides Shop Gateway order-credit operations.</summary>
public interface IWalletClient
{
    /// <summary>Gets the credit balance available for creating orders.</summary>
    Task<OrderCredit> GetOrderCreditAsync(
        CancellationToken cancellationToken = default);
}
