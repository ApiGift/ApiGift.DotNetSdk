using ApiGift.Sdk.Contracts.V2.Categories;

namespace ApiGift.Sdk.V2.Categories;

/// <summary>Provides product-category operations.</summary>
public interface ICategoriesClient
{
    /// <summary>Gets the product categories and catalog navigation data.</summary>
    Task<IReadOnlyList<ProductCategory>> GetCategoriesAsync(
        CancellationToken cancellationToken = default);
}
