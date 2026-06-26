using ApiGift.Sdk.Contracts.V2.Inventory;

namespace ApiGift.Sdk.V2.Inventory;

/// <summary>Provides takeout inventory and return-workflow operations.</summary>
public interface IInventoryClient
{
    /// <summary>Creates an inventory takeout request.</summary>
    Task<CreateTakeoutResponse> CreateTakeoutAsync(
        CreateTakeoutRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Cancels a pending takeout request.</summary>
    Task CancelTakeoutAsync(
        Guid takeoutInventoryId,
        CancellationToken cancellationToken = default);

    /// <summary>Gets takeout status by delivery key.</summary>
    Task<TakeoutStatus> GetTakeoutStatusAsync(
        string deliveryKey,
        CancellationToken cancellationToken = default);

    /// <summary>Creates or updates an inventory return request.</summary>
    Task<CreateReturnResponse> CreateReturnAsync(
        CreateReturnRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Creates a warning return request.</summary>
    Task<CreateReturnResponse> CreateReturnWarningAsync(
        CreateReturnWarningRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Adds a reply to an inventory return request.</summary>
    Task<ReplyToReturnResponse> ReplyToReturnAsync(
        ReplyToReturnRequest request,
        CancellationToken cancellationToken = default);

    /// <summary>Starts mediation for an inventory return request.</summary>
    Task StartReturnMediationAsync(
        Guid inventoryId,
        CancellationToken cancellationToken = default);

    /// <summary>Closes an inventory return request.</summary>
    Task CloseReturnAsync(
        Guid inventoryId,
        CancellationToken cancellationToken = default);

    /// <summary>Gets inventory return details.</summary>
    Task<ReturnDetails> GetReturnDetailsAsync(
        Guid inventoryId,
        CancellationToken cancellationToken = default);
}
