using ApiGift.Sdk.V2;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiGift.Sdk.Tests;

public sealed class ApiGiftIntegrationTests
{
    [IntegrationFact]
    [Trait("Category", "Integration")]
    public async Task GetCategoriesReturnsSuccessfully()
    {
        ServiceCollection services = new();
        services.AddApiGiftSdk(options =>
        {
            options.BaseUrl = GetRequiredEnvironmentVariable("APIGIFT_BASE_URL");
            options.AccessKey =
                GetRequiredEnvironmentVariable("APIGIFT_ACCESS_KEY");
            options.SecretKey =
                GetRequiredEnvironmentVariable("APIGIFT_SECRET_KEY");
        });

        await using ServiceProvider serviceProvider =
            services.BuildServiceProvider();
        IApiGiftClient client =
            serviceProvider.GetRequiredService<IApiGiftClient>();
        using CancellationTokenSource cancellation =
            new(TimeSpan.FromSeconds(30));

        var categories = await client.Categories.GetCategoriesAsync(
            cancellation.Token);

        Assert.NotNull(categories);
    }

    private static string GetRequiredEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name)
            ?? throw new InvalidOperationException(
                $"Environment variable '{name}' is required.");
    }
}
