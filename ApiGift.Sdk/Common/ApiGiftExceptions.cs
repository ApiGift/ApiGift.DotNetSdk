using System.Net;

namespace ApiGift.Sdk.V2;

/// <summary>
/// Represents an unsuccessful ApiGift HTTP response.
/// </summary>
public class ApiGiftException : HttpRequestException
{
    /// <summary>Initializes an ApiGift exception.</summary>
    public ApiGiftException(
        string message,
        HttpStatusCode statusCode,
        string? responseBody = null,
        string? requestPath = null,
        Exception? innerException = null)
        : base(message, innerException, statusCode)
    {
        StatusCode = statusCode;
        ResponseBody = responseBody;
        RequestPath = requestPath;
    }

    /// <summary>Gets the HTTP response status code.</summary>
    public new HttpStatusCode StatusCode { get; }
    /// <summary>Gets the response body, when available.</summary>
    public string? ResponseBody { get; }
    /// <summary>Gets the request path and query, when available.</summary>
    public string? RequestPath { get; }
}

/// <summary>Represents an HTTP 400 validation response.</summary>
public class ApiGiftValidationException : ApiGiftException
{
    /// <summary>Initializes the exception.</summary>
    public ApiGiftValidationException(
        string message,
        string? responseBody = null,
        string? requestPath = null)
        : base(message, HttpStatusCode.BadRequest, responseBody, requestPath)
    {
    }
}

/// <summary>Represents an HTTP 401 authentication response.</summary>
public class ApiGiftUnauthorizedException : ApiGiftException
{
    /// <summary>Initializes the exception.</summary>
    public ApiGiftUnauthorizedException(
        string message,
        string? responseBody = null,
        string? requestPath = null)
        : base(message, HttpStatusCode.Unauthorized, responseBody, requestPath)
    {
    }
}

/// <summary>Represents an HTTP 403 authorization response.</summary>
public class ApiGiftForbiddenException : ApiGiftException
{
    /// <summary>Initializes the exception.</summary>
    public ApiGiftForbiddenException(
        string message,
        string? responseBody = null,
        string? requestPath = null)
        : base(message, HttpStatusCode.Forbidden, responseBody, requestPath)
    {
    }
}

/// <summary>Represents an HTTP 404 not-found response.</summary>
public class ApiGiftNotFoundException : ApiGiftException
{
    /// <summary>Initializes the exception.</summary>
    public ApiGiftNotFoundException(
        string message,
        string? responseBody = null,
        string? requestPath = null)
        : base(message, HttpStatusCode.NotFound, responseBody, requestPath)
    {
    }
}

/// <summary>Represents an HTTP 429 rate-limit response.</summary>
public class ApiGiftRateLimitException : ApiGiftException
{
    /// <summary>Initializes the exception.</summary>
    public ApiGiftRateLimitException(
        string message,
        string? responseBody = null,
        string? requestPath = null)
        : base(message, HttpStatusCode.TooManyRequests, responseBody, requestPath)
    {
    }
}

/// <summary>
/// Legacy exception retained for source compatibility with the initial SDK.
/// </summary>
[Obsolete("Use ApiGiftException or a status-specific ApiGift exception.")]
public sealed class ApiGiftApiException : ApiGiftException
{
    /// <summary>Initializes the legacy exception.</summary>
    public ApiGiftApiException(
        HttpStatusCode statusCode,
        string? responseBody,
        string message,
        string? requestPath = null)
        : base(message, statusCode, responseBody, requestPath)
    {
    }
}
