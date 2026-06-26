using ApiGift.Sdk.V2.Authentication;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace ApiGift.Sdk.V2;

internal sealed class ApiGiftHttpClient
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web)
    {
        PropertyNameCaseInsensitive = true
    };

    private readonly HttpClient httpClient;
    private readonly IApiGiftAuthenticationProvider authenticationProvider;
    private readonly Uri baseAddress;

    public ApiGiftHttpClient(
        HttpClient httpClient,
        ApiGiftClientOptions options,
        IApiGiftAuthenticationProvider authenticationProvider)
    {
        this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        ArgumentNullException.ThrowIfNull(options);
        this.authenticationProvider = authenticationProvider
            ?? throw new ArgumentNullException(nameof(authenticationProvider));

        Uri configuredBaseAddress = options.GetBaseAddress();
        if (!configuredBaseAddress.IsAbsoluteUri)
        {
            throw new ArgumentException(
                "BaseUrl or BaseAddress must be an absolute URI.",
                nameof(options));
        }

        baseAddress = new Uri($"{configuredBaseAddress.AbsoluteUri.TrimEnd('/')}/");
    }

    public async Task<T> SendAsync<T>(
        HttpMethod method,
        string relativeUri,
        object? requestBody,
        CancellationToken cancellationToken,
        IReadOnlyCollection<HttpStatusCode>? allowedErrorStatusCodes = null)
    {
        using HttpRequestMessage request = await CreateRequestAsync(
            method,
            relativeUri,
            requestBody,
            cancellationToken);
        using HttpResponseMessage response = await httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        bool shouldDeserialize = response.IsSuccessStatusCode
            || allowedErrorStatusCodes?.Contains(response.StatusCode) == true;

        if (!shouldDeserialize)
        {
            await ThrowApiExceptionAsync(request, response, cancellationToken);
        }

        string? responseBody = await ReadResponseBodyAsync(
            response,
            cancellationToken);

        if (string.IsNullOrWhiteSpace(responseBody))
        {
            throw new ApiGiftException(
                $"ApiGift returned an empty JSON response for {method} {relativeUri}.",
                response.StatusCode,
                responseBody,
                request.RequestUri?.PathAndQuery);
        }

        try
        {
            T? result = JsonSerializer.Deserialize<T>(
                responseBody,
                SerializerOptions);
            if (result is not null)
            {
                return result;
            }
        }
        catch (JsonException exception)
        {
            throw new ApiGiftException(
                $"ApiGift returned invalid JSON for {method} {relativeUri}.",
                response.StatusCode,
                responseBody,
                request.RequestUri?.PathAndQuery,
                exception);
        }

        throw new ApiGiftException(
            $"ApiGift returned a JSON null response for {method} {relativeUri}.",
            response.StatusCode,
            responseBody,
            request.RequestUri?.PathAndQuery);
    }

    public async Task SendAsync(
        HttpMethod method,
        string relativeUri,
        object? requestBody,
        CancellationToken cancellationToken)
    {
        using HttpRequestMessage request = await CreateRequestAsync(
            method,
            relativeUri,
            requestBody,
            cancellationToken);
        using HttpResponseMessage response = await httpClient.SendAsync(
            request,
            HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            await ThrowApiExceptionAsync(request, response, cancellationToken);
        }
    }

    private async ValueTask<HttpRequestMessage> CreateRequestAsync(
        HttpMethod method,
        string relativeUri,
        object? requestBody,
        CancellationToken cancellationToken)
    {
        Uri requestUri = new(baseAddress, relativeUri);
        HttpRequestMessage request = new(method, requestUri);
        request.Headers.TryAddWithoutValidation(
            "User-Agent",
            $"ApiGift.DotNetSdk/{ApiGiftSdkVersion.Current}");
        request.Headers.TryAddWithoutValidation(
            "X-SDK-Version",
            ApiGiftSdkVersion.Current);

        if (requestBody is not null)
        {
            request.Content = JsonContent.Create(requestBody, options: SerializerOptions);
        }

        try
        {
            await authenticationProvider.ApplyAuthenticationAsync(
                request,
                cancellationToken);
        }
        catch
        {
            request.Dispose();
            throw;
        }

        return request;
    }

    private static async Task ThrowApiExceptionAsync(
        HttpRequestMessage request,
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        string? responseBody = await ReadResponseBodyAsync(response, cancellationToken);
        string? requestPath = request.RequestUri?.PathAndQuery;
        string message =
            $"ApiGift returned HTTP {(int)response.StatusCode} ({response.ReasonPhrase}).";

        throw response.StatusCode switch
        {
            HttpStatusCode.BadRequest => new ApiGiftValidationException(
                message,
                responseBody,
                requestPath),
            HttpStatusCode.Unauthorized => new ApiGiftUnauthorizedException(
                message,
                responseBody,
                requestPath),
            HttpStatusCode.Forbidden => new ApiGiftForbiddenException(
                message,
                responseBody,
                requestPath),
            HttpStatusCode.NotFound => new ApiGiftNotFoundException(
                message,
                responseBody,
                requestPath),
            HttpStatusCode.TooManyRequests => new ApiGiftRateLimitException(
                message,
                responseBody,
                requestPath),
            _ => new ApiGiftException(
                message,
                response.StatusCode,
                responseBody,
                requestPath)
        };
    }

    private static async Task<string?> ReadResponseBodyAsync(
        HttpResponseMessage response,
        CancellationToken cancellationToken)
    {
        return response.Content is null
            ? null
            : await response.Content.ReadAsStringAsync(cancellationToken);
    }
}
