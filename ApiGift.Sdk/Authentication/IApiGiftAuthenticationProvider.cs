namespace ApiGift.Sdk.V2.Authentication;

/// <summary>
/// Applies authentication information to an outgoing ApiGift request.
/// </summary>
public interface IApiGiftAuthenticationProvider
{
    /// <summary>
    /// Applies authentication headers or other authentication state.
    /// </summary>
    /// <param name="request">The outgoing HTTP request.</param>
    /// <param name="cancellationToken">The operation cancellation token.</param>
    ValueTask ApplyAuthenticationAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken = default);
}
