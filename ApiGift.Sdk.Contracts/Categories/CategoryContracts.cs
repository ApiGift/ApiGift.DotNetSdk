using ApiGift.Sdk.Contracts.V2.Common;

namespace ApiGift.Sdk.Contracts.V2.Categories;

/// <summary>Specifies options for product-category retrieval.</summary>
public sealed class GetCategoriesRequest
{
    /// <summary>Gets or sets the language codes to return translations for. When null or empty, translations are not returned.</summary>
    public IReadOnlyCollection<string>? TranslationLanguages { get; set; }
}

/// <summary>Represents a product category in the v2 catalog.</summary>
public sealed class ProductCategory
{
    /// <summary>Gets or sets the category identifier.</summary>
    public Guid Id { get; set; }
    /// <summary>Gets or sets the category title.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets the category image name.</summary>
    public string ImageName { get; set; } = string.Empty;
    /// <summary>Gets or sets the parent category identifier.</summary>
    public Guid? ParentId { get; set; }
    /// <summary>Gets or sets the regions available for the category.</summary>
    public List<CategoryRegion> AvailableRegions { get; set; } = [];
    /// <summary>Gets or sets the product families in the category.</summary>
    public List<ProductFamily> Families { get; set; } = [];
    /// <summary>Gets or sets translations for the requested languages. Null when no translation languages were requested.</summary>
    public List<CategoryTranslation>? Translations { get; set; }
}

/// <summary>Represents a category or family translation for one language.</summary>
public sealed class CategoryTranslation
{
    /// <summary>Gets or sets the language code (e.g. en, fa, ar).</summary>
    public string LanguageCode { get; set; } = string.Empty;
    /// <summary>Gets or sets the translated title.</summary>
    public string Title { get; set; } = string.Empty;
}

/// <summary>Represents a region translation for one language.</summary>
public sealed class CategoryRegionTranslation
{
    /// <summary>Gets or sets the language code (e.g. en, fa, ar).</summary>
    public string LanguageCode { get; set; } = string.Empty;
    /// <summary>Gets or sets the translated region name.</summary>
    public string Name { get; set; } = string.Empty;
}

/// <summary>Represents a region available in catalog navigation.</summary>
public sealed class CategoryRegion
{
    /// <summary>Gets or sets the region identifier.</summary>
    public Guid Id { get; set; }
    /// <summary>Gets or sets the region name.</summary>
    public string Name { get; set; } = string.Empty;
    /// <summary>Gets or sets the region code.</summary>
    public string Code { get; set; } = string.Empty;
    /// <summary>Gets or sets the optional flag image URL.</summary>
    public string? FlagImageUrl { get; set; }
    /// <summary>Gets or sets translations for the requested languages. Null when no translation languages were requested.</summary>
    public List<CategoryRegionTranslation>? Translations { get; set; }
}

/// <summary>Represents a product family in a category.</summary>
public sealed class ProductFamily
{
    /// <summary>Gets or sets the family identifier.</summary>
    public Guid Id { get; set; }
    /// <summary>Gets or sets the owning category identifier.</summary>
    public Guid CategoryId { get; set; }
    /// <summary>Gets or sets the family title.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets the optional family image name.</summary>
    public string? ImageName { get; set; }
    /// <summary>Gets or sets the regions available for the family.</summary>
    public List<CategoryRegion> AvailableRegions { get; set; } = [];
    /// <summary>Gets or sets the family variant type.</summary>
    public ProductVariantType? VariantType { get; set; }
    /// <summary>Gets or sets translations for the requested languages. Null when no translation languages were requested.</summary>
    public List<CategoryTranslation>? Translations { get; set; }
}
