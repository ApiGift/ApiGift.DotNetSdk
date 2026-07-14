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
    public Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync(
        CancellationToken cancellationToken = default)
    {
        return GetCategoriesAsync(null, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync(
        GetCategoriesRequest? request,
        CancellationToken cancellationToken = default)
    {
        return await httpClient.SendAsync<List<ProductCategory>>(
            HttpMethod.Get,
            BuildCategoriesUri(request),
            null,
            cancellationToken);
    }

    private static string BuildCategoriesUri(GetCategoriesRequest? request)
    {
        if (request?.TranslationLanguages is null)
        {
            return "v2/ProductCategory";
        }

        List<string> query = request.TranslationLanguages
            .Where(language => !string.IsNullOrWhiteSpace(language))
            .Select(language => $"translationLanguages={Uri.EscapeDataString(language)}")
            .ToList();

        return query.Count == 0
            ? "v2/ProductCategory"
            : $"v2/ProductCategory?{string.Join("&", query)}";
    }
}
