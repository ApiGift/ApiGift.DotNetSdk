using ApiGift.Sdk.Contracts.V2.Common;

namespace ApiGift.Sdk.Contracts.V2.Products;

/// <summary>Specifies filters for product catalog retrieval.</summary>
public sealed class GetProductsRequest
{
    /// <summary>Gets or sets a legacy single-category filter.</summary>
    public Guid? CategoryId { get; set; }
    /// <summary>Gets or sets category filters.</summary>
    public IReadOnlyCollection<Guid>? CategoryIds { get; set; }
    /// <summary>Gets or sets the product-family filter.</summary>
    public Guid? ProductFamilyId { get; set; }
    /// <summary>Gets or sets the region filter.</summary>
    public Guid? RegionId { get; set; }
    /// <summary>Gets or sets the free-text search term.</summary>
    public string? Search { get; set; }
    /// <summary>Gets or sets the language codes to return translations for. When null or empty, product translations are not returned.</summary>
    public IReadOnlyCollection<string>? TranslationLanguages { get; set; }
}

/// <summary>Represents a product returned by the v2 catalog.</summary>
public sealed class ProductSummary
{
    /// <summary>Gets or sets the product identifier.</summary>
    public Guid Id { get; set; }
    /// <summary>Gets or sets the product title.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets the category identifier.</summary>
    public Guid CategoryId { get; set; }
    /// <summary>Gets or sets the product-family identifier.</summary>
    public Guid? ProductFamilyId { get; set; }
    /// <summary>Gets or sets the product-family title.</summary>
    public string? ProductFamilyTitle { get; set; }
    /// <summary>Gets or sets the region identifier.</summary>
    public Guid? RegionId { get; set; }
    /// <summary>Gets or sets the region title.</summary>
    public string? RegionTitle { get; set; }
    /// <summary>Gets or sets the fulfillment method.</summary>
    public ProductFulfillmentMethod FulfillmentMethod { get; set; }
    /// <summary>Gets or sets variant information.</summary>
    public ProductVariant Variant { get; set; } = new();
    /// <summary>Gets or sets product-specific input/identity field definitions.</summary>
    public ProductDefinition Definition { get; set; } = new();
    /// <summary>Gets or sets translations for the requested languages. Null when no translation languages were requested.</summary>
    public List<ProductTranslation>? Translations { get; set; }
}

/// <summary>Represents a product translation for one language.</summary>
public sealed class ProductTranslation
{
    /// <summary>Gets or sets the language code (e.g. en, fa, ar).</summary>
    public string LanguageCode { get; set; } = string.Empty;
    /// <summary>Gets or sets the translated product title.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets the translated variant display name.</summary>
    public string? VariantDisplayName { get; set; }
}

/// <summary>Identifies how a product is fulfilled.</summary>
public enum ProductFulfillmentMethod
{
    Inventory = 1,
    Activation = 2
}

/// <summary>Represents a product variant value and display information.</summary>
public sealed class ProductVariant
{
    /// <summary>Gets or sets the variant type.</summary>
    public ProductVariantType? Type { get; set; }
    /// <summary>Gets or sets the variant value.</summary>
    public decimal? Value { get; set; }
    /// <summary>Gets or sets the variant unit.</summary>
    public string? Unit { get; set; }
    /// <summary>Gets or sets the display name.</summary>
    public string? DisplayName { get; set; }
}

/// <summary>Represents product-specific field definitions used by activation flows.</summary>
public sealed class ProductDefinition
{
    /// <summary>Gets or sets the fields required or supported by this product.</summary>
    public List<ProductFieldDefinition> Fields { get; set; } = [];
    /// <summary>Gets or sets the inputs required before purchasing this product.</summary>
    public List<ProductPurchaseInputDefinition> PurchaseInputs { get; set; } = [];
}

/// <summary>Represents one product-specific field definition.</summary>
public sealed class ProductFieldDefinition
{
    /// <summary>Gets or sets the field key used in API request payloads.</summary>
    public string Key { get; set; } = string.Empty;
    /// <summary>Gets or sets the display title.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets whether this field value must be unique.</summary>
    public bool IsUniqueValue { get; set; }
    /// <summary>Gets or sets optional usage guidance for this field.</summary>
    public string? Description { get; set; }
}

/// <summary>Represents one product purchase input definition.</summary>
public sealed class ProductPurchaseInputDefinition
{
    /// <summary>Gets or sets the input key used in API request payloads.</summary>
    public string Key { get; set; } = string.Empty;
    /// <summary>Gets or sets whether this input is required.</summary>
    public bool Required { get; set; }
    /// <summary>Gets or sets optional regex validation.</summary>
    public string? Pattern { get; set; }
    /// <summary>Gets or sets technical guidance for integrations.</summary>
    public string? TechnicalHint { get; set; }
    /// <summary>Gets or sets human-friendly guidance for end users.</summary>
    public string? UserHint { get; set; }
    /// <summary>Gets or sets the display name.</summary>
    public string? DisplayName { get; set; }
}

/// <summary>Represents merchant inventory availability for a product.</summary>
public sealed class ProductInventoryStatus
{
    /// <summary>Gets or sets the product identifier.</summary>
    public Guid ProductId { get; set; }
    /// <summary>Gets or sets the available merchant inventory count.</summary>
    public int MerchantInventoryAvailableCount { get; set; }
}

/// <summary>Represents current product market and supply status.</summary>
public sealed class ProductMarketStatus
{
    /// <summary>Gets or sets the product identifier.</summary>
    public Guid ProductId { get; set; }
    /// <summary>Gets or sets the product title.</summary>
    public string Title { get; set; } = string.Empty;
    /// <summary>Gets or sets the current supply price.</summary>
    public double CurrentSupplyPrice { get; set; }
    /// <summary>Gets or sets market availability.</summary>
    public ProductAvailabilityStatus AvailabilityStatus { get; set; }
    /// <summary>Gets or sets product-specific input/identity field definitions.</summary>
    public ProductDefinition Definition { get; set; } = new();
    /// <summary>Gets or sets automatic supply information.</summary>
    public ProductAutoSupplyInfo AutoSupply { get; set; } = new();
}

/// <summary>Identifies product market availability.</summary>
public enum ProductAvailabilityStatus
{
    Available,
    AvailableWithDelay,
    Unavailable
}

/// <summary>Represents automatic supply availability.</summary>
public sealed class ProductAutoSupplyInfo
{
    /// <summary>Gets or sets whether automatic supply is available.</summary>
    public bool IsAvailable { get; set; }
    /// <summary>Gets or sets the reason automatic supply is blocked.</summary>
    public ProductAutoSupplyBlockReason? BlockReason { get; set; }
}

/// <summary>Identifies why automatic supply is unavailable.</summary>
public enum ProductAutoSupplyBlockReason
{
    MarketPriceAboveMaximumPrice = 1
}
