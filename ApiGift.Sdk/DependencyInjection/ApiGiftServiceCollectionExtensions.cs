using ApiGift.Sdk.V2.Authentication;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ApiGift.Sdk.V2;

/// <summary>
/// Dependency-injection registration methods for the ApiGift SDK.
/// </summary>
public static class ApiGiftServiceCollectionExtensions
{
    private const string HttpClientName = "ApiGift.DotNetSdk";

    /// <summary>
    /// Registers the ApiGift Shop Gateway v2 client and its HTTP dependencies.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <param name="configure">The SDK options configuration callback.</param>
    /// <returns>The supplied service collection.</returns>
    public static IServiceCollection AddApiGiftSdk(
        this IServiceCollection services,
        Action<ApiGiftClientOptions> configure)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configure);

        ApiGiftClientOptions options = new();
        configure(options);
        _ = options.GetBaseAddress();

        services.TryAddSingleton(options);
        services.TryAddSingleton<IApiGiftAuthenticationProvider>(
            new AccessKeySecretKeyAuthenticationProvider(
                options.AccessKey,
                options.SecretKey));

        services.AddHttpClient(HttpClientName);
        services.TryAddTransient(serviceProvider =>
        {
            IHttpClientFactory httpClientFactory =
                serviceProvider.GetRequiredService<IHttpClientFactory>();

            return new ApiGiftHttpClient(
                httpClientFactory.CreateClient(HttpClientName),
                serviceProvider.GetRequiredService<ApiGiftClientOptions>(),
                serviceProvider.GetRequiredService<IApiGiftAuthenticationProvider>());
        });
        services.TryAddTransient<IApiGiftClient>(
            serviceProvider => new ApiGiftClient(
                serviceProvider.GetRequiredService<ApiGiftHttpClient>()));

        return services;
    }
}
