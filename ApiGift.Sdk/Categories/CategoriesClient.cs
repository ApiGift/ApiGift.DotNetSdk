using ApiGift.Sdk.Contracts.V2.Categories;

namespace ApiGift.Sdk.V2.Categories;

/// <inheritdoc />
public sealed class CategoriesClient : ICategoriesClient
{
    private readonly ApiGiftHttpClient httpClient;

    internal CategoriesClient(ApiGiftHttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return await httpClient.SendAsync<List<ProductCategory>>(
            HttpMethod.Get,
            "v2/ProductCategory",
            null,
            cancellationToken);
    }
}
