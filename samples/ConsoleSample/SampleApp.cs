using ApiGift.Sdk.V2;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleSample;

internal sealed partial class SampleApp
{
    private readonly IApiGiftClient client;
    private readonly string baseUrl;

    private SampleApp(IApiGiftClient client, string baseUrl)
    {
        this.client = client;
        this.baseUrl = baseUrl;
    }

    public static SampleApp Create()
    {
        string baseUrl = GetRequiredEnvironmentVariable("APIGIFT_BASE_URL");
        string accessKey = GetRequiredEnvironmentVariable("APIGIFT_ACCESS_KEY");
        string secretKey = GetRequiredEnvironmentVariable("APIGIFT_SECRET_KEY");

        ServiceCollection services = new();
        services.AddApiGiftSdk(options =>
        {
            options.BaseUrl = baseUrl;
            options.AccessKey = accessKey;
            options.SecretKey = secretKey;
        });

        ServiceProvider serviceProvider = services.BuildServiceProvider();
        IApiGiftClient client = serviceProvider.GetRequiredService<IApiGiftClient>();

        return new SampleApp(client, baseUrl);
    }

    public async Task RunAsync()
    {
        Console.WriteLine();
        Console.WriteLine("ApiGift SDK sample is ready.");
        Console.WriteLine($"Base URL: {baseUrl}");

        while (true)
        {
            PrintMenu();

            string choice = ConsoleInput.ReadRequired("Select an option");
            Console.WriteLine();

            if (choice == "0")
                return;

            await RunSafelyAsync(GetAction(choice));
        }
    }

    private static void PrintMenu()
    {
        Console.WriteLine();
        Console.WriteLine("Main menu");
        Console.WriteLine("  1. Wallet - get order credit");
        Console.WriteLine("  2. Catalog - list categories");
        Console.WriteLine("  3. Catalog - list products");
        Console.WriteLine("  4. Products - get inventory status");
        Console.WriteLine("  5. Products - get market status");
        Console.WriteLine("  6. Inventory - get takeout status");
        Console.WriteLine("  7. Returns - get details");
        Console.WriteLine("  8. Subscriptions - get status");
        Console.WriteLine("  0. Exit");
    }

    private Func<Task> GetAction(string choice) =>
        choice switch
        {
            "1" => GetWalletBalanceAsync,
            "2" => ListCategoriesAsync,
            "3" => ListProductsAsync,
            "4" => GetInventoryStatusAsync,
            "5" => GetMarketStatusAsync,
            "6" => GetTakeoutStatusAsync,
            "7" => GetReturnDetailsAsync,
            "8" => GetSubscriptionStatusAsync,
            _ => () =>
            {
                ConsoleOutput.WriteError("Unknown option.");
                return Task.CompletedTask;
            }
        };

    private static async Task RunSafelyAsync(Func<Task> action)
    {
        try
        {
            await action();
        }
        catch (ApiGiftException exception)
        {
            ConsoleOutput.WriteError(
                $"ApiGift request failed with HTTP {(int)exception.StatusCode}.");
            Console.WriteLine($"Path: {exception.RequestPath}");
            Console.WriteLine($"Response: {exception.ResponseBody}");
        }
        catch (OperationCanceledException)
        {
            ConsoleOutput.WriteError("The request was canceled or timed out.");
        }
        catch (Exception exception)
        {
            ConsoleOutput.WriteError(exception.Message);
        }

        Console.WriteLine();
        Console.WriteLine("Press Enter to continue...");
        Console.ReadLine();
    }

    private async Task GetWalletBalanceAsync()
    {
        var credit = await client.Wallet.GetOrderCreditAsync();
        Console.WriteLine($"Order credit balance: {credit.Balance}");
    }

    private static string GetRequiredEnvironmentVariable(string name)
    {
        return Environment.GetEnvironmentVariable(name)
            ?? throw new InvalidOperationException(
                $"Set the {name} environment variable before running this sample.");
    }
}
