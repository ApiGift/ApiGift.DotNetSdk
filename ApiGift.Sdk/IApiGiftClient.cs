using ApiGift.Sdk.V2.Categories;
using ApiGift.Sdk.V2.Inventory;
using ApiGift.Sdk.V2.Merchant;
using ApiGift.Sdk.V2.Orders;
using ApiGift.Sdk.V2.Products;
using ApiGift.Sdk.V2.Wallet;

namespace ApiGift.Sdk.V2;

/// <summary>
/// Provides access to the ApiGift Shop Gateway v2 feature clients.
/// </summary>
public interface IApiGiftClient
{
    /// <summary>Gets the product-category client.</summary>
    ICategoriesClient Categories { get; }
    /// <summary>Gets the product catalog client.</summary>
    IProductsClient Products { get; }
    /// <summary>Gets the inventory and return-workflow client.</summary>
    IInventoryClient Inventory { get; }
    /// <summary>Gets the subscription order client.</summary>
    IOrdersClient Orders { get; }
    /// <summary>Gets the merchant client reserved for the v2 public surface.</summary>
    IMerchantClient Merchant { get; }
    /// <summary>Gets the order-credit wallet client.</summary>
    IWalletClient Wallet { get; }
}
