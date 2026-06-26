using ApiGift.Sdk.V2.Authentication;
using ApiGift.Sdk.V2.Categories;
using ApiGift.Sdk.V2.Inventory;
using ApiGift.Sdk.V2.Merchant;
using ApiGift.Sdk.V2.Orders;
using ApiGift.Sdk.V2.Products;
using ApiGift.Sdk.V2.Wallet;

namespace ApiGift.Sdk.V2;

/// <summary>
/// Default ApiGift Shop Gateway v2 client.
/// </summary>
public sealed class ApiGiftClient : IApiGiftClient
{
    /// <summary>
    /// Initializes a client using access-key and secret-key authentication from
    /// the supplied options.
    /// </summary>
    public ApiGiftClient(HttpClient httpClient, ApiGiftClientOptions options)
        : this(
            httpClient,
            options,
            CreateDefaultAuthenticationProvider(options))
    {
    }

    /// <summary>
    /// Initializes a client using a custom authentication provider.
    /// </summary>
    public ApiGiftClient(
        HttpClient httpClient,
        ApiGiftClientOptions options,
        IApiGiftAuthenticationProvider authenticationProvider)
    {
        ApiGiftHttpClient apiGiftHttpClient = new(
            httpClient,
            options,
            authenticationProvider);

        Categories = new CategoriesClient(apiGiftHttpClient);
        Products = new ProductsClient(apiGiftHttpClient);
        Inventory = new InventoryClient(apiGiftHttpClient);
        Orders = new OrdersClient(apiGiftHttpClient);
        Merchant = new MerchantClient();
        Wallet = new WalletClient(apiGiftHttpClient);
    }

    internal ApiGiftClient(ApiGiftHttpClient apiGiftHttpClient)
    {
        Categories = new CategoriesClient(apiGiftHttpClient);
        Products = new ProductsClient(apiGiftHttpClient);
        Inventory = new InventoryClient(apiGiftHttpClient);
        Orders = new OrdersClient(apiGiftHttpClient);
        Merchant = new MerchantClient();
        Wallet = new WalletClient(apiGiftHttpClient);
    }

    /// <inheritdoc />
    public ICategoriesClient Categories { get; }
    /// <inheritdoc />
    public IProductsClient Products { get; }
    /// <inheritdoc />
    public IInventoryClient Inventory { get; }
    /// <inheritdoc />
    public IOrdersClient Orders { get; }
    /// <inheritdoc />
    public IMerchantClient Merchant { get; }
    /// <inheritdoc />
    public IWalletClient Wallet { get; }

    private static IApiGiftAuthenticationProvider CreateDefaultAuthenticationProvider(
        ApiGiftClientOptions options)
    {
        ArgumentNullException.ThrowIfNull(options);

        return new AccessKeySecretKeyAuthenticationProvider(
            options.AccessKey,
            options.SecretKey);
    }
}
