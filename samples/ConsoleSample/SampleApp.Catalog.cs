using ApiGift.Sdk.Contracts.V2.Categories;
using ApiGift.Sdk.Contracts.V2.Products;

namespace ConsoleSample;

internal sealed partial class SampleApp
{
    private async Task ListCategoriesAsync()
    {
        IReadOnlyList<ProductCategory> categories =
            await client.Categories.GetCategoriesAsync();

        ConsoleOutput.WriteTable(
            ["#", "Category ID", "Title", "Parent ID", "Families", "Regions"],
            categories.Take(50).Select((category, index) => new[]
            {
                (index + 1).ToString(),
                category.Id.ToString(),
                category.Title,
                category.ParentId?.ToString() ?? "-",
                category.Families.Count.ToString(),
                category.AvailableRegions.Count.ToString()
            }));

        Console.WriteLine(
            $"Showing {Math.Min(categories.Count, 50)} of {categories.Count} categories.");
    }

    private async Task ListProductsAsync()
    {
        GetProductsRequest request = new()
        {
            CategoryId = ConsoleInput.ReadOptionalGuid("Category ID"),
            ProductFamilyId = ConsoleInput.ReadOptionalGuid("Product family ID"),
            RegionId = ConsoleInput.ReadOptionalGuid("Region ID"),
            Search = ConsoleInput.ReadOptional("Search")
        };

        string categoryIds = ConsoleInput.ReadOptional(
            "Additional category IDs, comma separated");
        if (!string.IsNullOrWhiteSpace(categoryIds))
        {
            request.CategoryIds = categoryIds
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(Guid.Parse)
                .ToList();
        }

        IReadOnlyList<ProductSummary> products =
            await client.Products.GetProductsAsync(request);

        ConsoleOutput.WriteTable(
            ["#", "Product ID", "Title", "Family", "Region", "Fulfillment", "Fields"],
            products.Take(50).Select((product, index) => new[]
            {
                (index + 1).ToString(),
                product.Id.ToString(),
                product.Title,
                product.ProductFamilyTitle ?? "-",
                product.RegionTitle ?? "-",
                product.FulfillmentMethod.ToString(),
                product.Definition.Fields.Count.ToString()
            }));

        Console.WriteLine(
            $"Showing {Math.Min(products.Count, 50)} of {products.Count} products.");

        ProductSummary? selectedProduct = SelectProduct(products);
        if (selectedProduct is not null)
            PrintProductDetails(selectedProduct);
    }

    private async Task GetInventoryStatusAsync()
    {
        Guid productId = ConsoleInput.ReadGuid("Product ID");
        ProductInventoryStatus status =
            await client.Products.GetInventoryStatusAsync(productId);

        Console.WriteLine($"Product ID: {status.ProductId}");
        Console.WriteLine(
            $"Merchant inventory available: {status.MerchantInventoryAvailableCount}");
    }

    private async Task GetMarketStatusAsync()
    {
        Guid productId = ConsoleInput.ReadGuid("Product ID");
        ProductMarketStatus status =
            await client.Products.GetMarketStatusAsync(productId);

        Console.WriteLine($"Product ID: {status.ProductId}");
        Console.WriteLine($"Title: {status.Title}");
        Console.WriteLine($"Current supply price: {status.CurrentSupplyPrice}");
        Console.WriteLine($"Availability: {status.AvailabilityStatus}");
        Console.WriteLine($"Auto supply available: {status.AutoSupply.IsAvailable}");
        Console.WriteLine(
            $"Auto supply block reason: {status.AutoSupply.BlockReason?.ToString() ?? "-"}");
    }

    private static ProductSummary? SelectProduct(IReadOnlyList<ProductSummary> products)
    {
        if (products.Count == 0)
            return null;

        string value = ConsoleInput.ReadOptional(
            "Enter product row number to show details, or press Enter");
        if (string.IsNullOrWhiteSpace(value))
            return null;

        if (!int.TryParse(value, out int row) || row < 1 || row > products.Count)
        {
            ConsoleOutput.WriteError("Invalid product row.");
            return null;
        }

        return products[row - 1];
    }

    private static void PrintProductDetails(ProductSummary product)
    {
        Console.WriteLine();
        Console.WriteLine("Product details");
        Console.WriteLine($"ID: {product.Id}");
        Console.WriteLine($"Title: {product.Title}");
        Console.WriteLine($"Category ID: {product.CategoryId}");
        Console.WriteLine(
            $"Family: {product.ProductFamilyTitle ?? "-"} ({product.ProductFamilyId?.ToString() ?? "-"})");
        Console.WriteLine(
            $"Region: {product.RegionTitle ?? "-"} ({product.RegionId?.ToString() ?? "-"})");
        Console.WriteLine($"Fulfillment: {product.FulfillmentMethod}");
        Console.WriteLine(
            $"Variant: {product.Variant.DisplayName ?? "-"} {product.Variant.Value?.ToString() ?? string.Empty} {product.Variant.Unit ?? string.Empty}".Trim());

        if (product.Definition.Fields.Count == 0)
        {
            Console.WriteLine("Definition fields: none");
            return;
        }

        Console.WriteLine("Definition fields:");
        ConsoleOutput.WriteTable(
            ["Key", "Title", "Unique", "Description"],
            product.Definition.Fields.Select(field => new[]
            {
                field.Key,
                field.Title,
                field.IsUniqueValue.ToString(),
                field.Description ?? "-"
            }));
    }
}
