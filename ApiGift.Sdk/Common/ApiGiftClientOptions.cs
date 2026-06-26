namespace ApiGift.Sdk.V2;

/// <summary>
/// Configures the ApiGift Shop Gateway v2 client.
/// </summary>
public sealed class ApiGiftClientOptions
{
    /// <summary>Gets or sets the absolute API base URL.</summary>
    public string BaseUrl { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the absolute API base address. This takes precedence over
    /// <see cref="BaseUrl"/>.
    /// </summary>
    public Uri? BaseAddress { get; set; }
    /// <summary>Gets or sets the Shop Gateway access key.</summary>
    public string AccessKey { get; set; } = string.Empty;
    /// <summary>Gets or sets the Shop Gateway secret key.</summary>
    public string SecretKey { get; set; } = string.Empty;

    internal Uri GetBaseAddress()
    {
        if (BaseAddress is not null)
        {
            return BaseAddress;
        }

        if (Uri.TryCreate(BaseUrl, UriKind.Absolute, out Uri? baseAddress))
        {
            return baseAddress;
        }

        throw new InvalidOperationException(
            "ApiGift SDK BaseUrl or BaseAddress must be an absolute URI.");
    }
}
