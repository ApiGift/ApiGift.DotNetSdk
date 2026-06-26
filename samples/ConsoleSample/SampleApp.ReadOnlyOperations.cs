using ApiGift.Sdk.Contracts.V2.Inventory;
using ApiGift.Sdk.Contracts.V2.Orders;

namespace ConsoleSample;

internal sealed partial class SampleApp
{
    private async Task GetTakeoutStatusAsync()
    {
        string deliveryKey = ConsoleInput.ReadRequired("Delivery key");
        TakeoutStatus status =
            await client.Inventory.GetTakeoutStatusAsync(deliveryKey);

        Console.WriteLine($"Takeout ID: {status.TakeoutInventoryId}");
        Console.WriteLine($"Inventory ID: {status.InventoryId?.ToString() ?? "-"}");
        Console.WriteLine($"Product ID: {status.ProductId}");
        Console.WriteLine($"Entry price: {status.EntryPrice?.ToString() ?? "-"}");
        Console.WriteLine($"Canceled: {status.IsCanceled}");
        Console.WriteLine($"Webhook error: {status.WebhookDeliveredError ?? "-"}");
        Console.WriteLine(
            $"Encrypted virtual collections: {ConsoleOutput.Trim(status.EncryptedVirtualCollections, 160)}");
    }

    private async Task GetReturnDetailsAsync()
    {
        Guid inventoryId = ConsoleInput.ReadGuid("Inventory ID");
        ReturnDetails details =
            await client.Inventory.GetReturnDetailsAsync(inventoryId);

        Console.WriteLine($"Support ticket ID: {details.SupportTicketId}");
        Console.WriteLine($"Ticket number: {details.TicketNumber}");
        Console.WriteLine($"Reason: {details.ReturnReasonCode}");
        Console.WriteLine($"Created at: {details.CreateAt:u}");
        Console.WriteLine($"Status: {details.Status}");
        Console.WriteLine($"Under mediation: {details.UnderMediation}");
        Console.WriteLine();

        ConsoleOutput.WriteTable(
            ["#", "Created", "Shop ID", "Event", "Text"],
            details.Replies.Select((reply, index) => new[]
            {
                (index + 1).ToString(),
                reply.CreateAt.ToString("u"),
                reply.ShopId?.ToString() ?? "-",
                reply.IsEvent.ToString(),
                ConsoleOutput.Trim(reply.Text, 80)
            }));
    }

    private async Task GetSubscriptionStatusAsync()
    {
        string referenceId = ConsoleInput.ReadRequired("Reference ID");
        SubscriptionStatus status =
            await client.Orders.GetSubscriptionStatusAsync(referenceId);

        Console.WriteLine($"Order ID: {status.OrderId}");
        Console.WriteLine($"Order status: {status.OrderStatus}");
        Console.WriteLine($"Price: {status.Price}");
    }
}
