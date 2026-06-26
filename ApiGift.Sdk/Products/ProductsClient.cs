using ApiGift.Sdk.Contracts.V2.Products;

namespace ApiGift.Sdk.V2.Products;

/// <inheritdoc />
public sealed class ProductsClient : IProductsClient
{
    private readonly ApiGiftHttpClient httpClient;

    internal ProductsClient(ApiGiftHttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProductSummary>> GetProductsAsync(
        GetProductsRequest? request = null,
        CancellationToken cancellationToken = default)
    {
        string relativeUri = BuildProductsUri(request);
        return await httpClient.SendAsync<List<ProductSummary>>(
            HttpMethod.Get,
            relativeUri,
            null,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ProductInventoryStatus> GetInventoryStatusAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<ProductInventoryStatus>(
            HttpMethod.Get,
            $"v2/Products/{productId:D}/InventoryStatus",
            null,
            cancellationToken);
    }

    /// <inheritdoc />
    public Task<ProductMarketStatus> GetMarketStatusAsync(
        Guid productId,
        CancellationToken cancellationToken = default)
    {
        return httpClient.SendAsync<ProductMarketStatus>(
            HttpMethod.Get,
            $"v2/Products/{productId:D}/MarketStatus",
            null,
            cancellationToken);
    }

    private static string BuildProductsUri(GetProductsRequest? request)
    {
        if (request is null)
        {
            return "v2/Products";
        }

        List<string> query = [];

        if (request.CategoryId.HasValue)
        {
            query.Add($"categoryId={request.CategoryId.Value:D}");
        }

        if (request.CategoryIds is not null)
        {
            query.AddRange(request.CategoryIds.Select(
                categoryId => $"categoryIds={categoryId:D}"));
        }

        if (request.ProductFamilyId.HasValue)
        {
            query.Add($"productFamilyId={request.ProductFamilyId.Value:D}");
        }

        if (request.RegionId.HasValue)
        {
            query.Add($"regionId={request.RegionId.Value:D}");
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query.Add($"search={Uri.EscapeDataString(request.Search)}");
        }

        return query.Count == 0
            ? "v2/Products"
            : $"v2/Products?{string.Join("&", query)}";
    }
}
