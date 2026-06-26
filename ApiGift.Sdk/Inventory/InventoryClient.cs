using ApiGift.Sdk.Contracts.V2.Inventory;

namespace ApiGift.Sdk.V2.Inventory;

/// <inheritdoc />
public sealed class InventoryClient : IInventoryClient
{
    private readonly ApiGiftHttpClient httpClient;

    internal InventoryClient(ApiGiftHttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <inheritdoc />
    public Task<CreateTakeoutResponse> CreateTakeoutAsync(
        CreateTakeoutRequest request,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<CreateTakeoutResponse>(
            HttpMethod.Post,
            "v2/TakeoutInventory",
            request,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task CancelTakeoutAsync(
        Guid takeoutInventoryId,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync(
            HttpMethod.Delete,
            $"v2/TakeoutInventory/{takeoutInventoryId:D}",
            null,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<TakeoutStatus> GetTakeoutStatusAsync(
        string deliveryKey,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(deliveryKey);

        return httpClient.SendAsync<TakeoutStatus>(
            HttpMethod.Get,
            $"v2/TakeoutInventory?deliveryKey={Uri.EscapeDataString(deliveryKey)}",
            null,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<CreateReturnResponse> CreateReturnAsync(
        CreateReturnRequest request,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<CreateReturnResponse>(
            HttpMethod.Post,
            "v2/ReturnInventory",
            request,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<CreateReturnResponse> CreateReturnWarningAsync(
        CreateReturnWarningRequest request,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<CreateReturnResponse>(
            HttpMethod.Post,
            "v2/ReturnInventory/Warning",
            request,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ReplyToReturnResponse> ReplyToReturnAsync(
        ReplyToReturnRequest request,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<ReplyToReturnResponse>(
            HttpMethod.Post,
            "v2/ReturnInventory/Reply",
            request,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task StartReturnMediationAsync(
        Guid inventoryId,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync(
            HttpMethod.Put,
            $"v2/ReturnInventory/StartMediation/{inventoryId:D}",
            null,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task CloseReturnAsync(
        Guid inventoryId,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync(
            HttpMethod.Put,
            $"v2/ReturnInventory/Close/{inventoryId:D}",
            null,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ReturnDetails> GetReturnDetailsAsync(
        Guid inventoryId,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<ReturnDetails>(
            HttpMethod.Get,
            $"v2/ReturnInventory/{inventoryId:D}",
            null,
            cancellationToken);
    }
}
