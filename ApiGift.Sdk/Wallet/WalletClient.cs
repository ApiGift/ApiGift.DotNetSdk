using ApiGift.Sdk.Contracts.V2.Wallet;

namespace ApiGift.Sdk.V2.Wallet;

/// <inheritdoc />
public sealed class WalletClient : IWalletClient
{
    private readonly ApiGiftHttpClient httpClient;

    internal WalletClient(ApiGiftHttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <inheritdoc />
    public Task<OrderCredit> GetOrderCreditAsync(
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<OrderCredit>(
            HttpMethod.Get,
            "v2/OrderCredit",
            null,
            cancellationToken);
    }
}
