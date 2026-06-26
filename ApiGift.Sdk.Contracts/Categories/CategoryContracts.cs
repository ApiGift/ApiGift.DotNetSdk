using ApiGift.Sdk.Contracts.V2.Common;

namespace ApiGift.Sdk.Contracts.V2.Categories;

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
}
