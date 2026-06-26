using ApiGift.Sdk.Contracts.V2.Products;

namespace ApiGift.Sdk.V2.Products;

/// <summary>Provides product catalog and availability operations.</summary>
public interface IProductsClient
{
    /// <summary>Gets products matching the supplied filters.</summary>
    Task<IReadOnlyList<ProductSummary>> GetProductsAsync(
        GetProductsRequest? request = null,
        CancellationToken cancellationToken = default);

    /// <summary>Gets merchant inventory availability for a product.</summary>
    Task<ProductInventoryStatus> GetInventoryStatusAsync(
        Guid productId,
        CancellationToken cancellationToken = default);

    /// <summary>Gets current market and supply status for a product.</summary>
    Task<ProductMarketStatus> GetMarketStatusAsync(
        Guid productId,
        CancellationToken cancellationToken = default);
}
