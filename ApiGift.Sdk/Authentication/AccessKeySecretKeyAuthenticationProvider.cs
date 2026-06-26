namespace ApiGift.Sdk.V2.Authentication;

/// <summary>
/// Applies ApiGift access-key and secret-key authentication headers.
/// </summary>
public sealed class AccessKeySecretKeyAuthenticationProvider
    : IApiGiftAuthenticationProvider
{
    /// <summary>Initializes the authentication provider.</summary>
    public AccessKeySecretKeyAuthenticationProvider(
        string accessKey,
        string secretKey)
    {
        if (string.IsNullOrWhiteSpace(accessKey))
        {
            throw new ArgumentException("Access key is required.", nameof(accessKey));
        }

        if (string.IsNullOrWhiteSpace(secretKey))
        {
            throw new ArgumentException("Secret key is required.", nameof(secretKey));
        }

        AccessKey = accessKey;
        SecretKey = secretKey;
    }

    /// <summary>Gets the configured access key.</summary>
    public string AccessKey { get; }
    /// <summary>Gets the configured secret key.</summary>
    public string SecretKey { get; }

    /// <inheritdoc />
    public ValueTask ApplyAuthenticationAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);
        cancellationToken.ThrowIfCancellationRequested();

        request.Headers.TryAddWithoutValidation("access-key", AccessKey);
        request.Headers.TryAddWithoutValidation("secret-key", SecretKey);

        return ValueTask.CompletedTask;
    }
}
