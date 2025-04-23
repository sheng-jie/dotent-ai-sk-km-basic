using Microsoft.Extensions.Http.Logging;
using Microsoft.Extensions.Logging;

public class KernelHttpClientLogger : IHttpClientAsyncLogger
{
    private readonly ILogger<KernelHttpClientLogger> _logger;

    public KernelHttpClientLogger(ILogger<KernelHttpClientLogger> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async ValueTask<object?> LogRequestStartAsync(HttpRequestMessage request,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var escapedString = await request.Content?.ReadAsStringAsync(cancellationToken)!;
        var content = System.Text.RegularExpressions.Regex.Unescape(escapedString);
        _logger.LogInformation(
            "Sending '{Request.Method}' to '{Request.Host}{Request.Path}' with content {Request.Content}",
            request.Method,
            request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
            request.RequestUri!.PathAndQuery,
            content);
        return null;
    }

    public async ValueTask LogRequestStopAsync(object? context, HttpRequestMessage request,
        HttpResponseMessage response,
        TimeSpan elapsed, CancellationToken cancellationToken = new CancellationToken())
    {
        var content = await response.Content.ReadAsStringAsync(cancellationToken);
        _logger.LogInformation(
            "Received '{Response.StatusCodeInt} {Response.StatusCodeString}' after {Response.ElapsedMilliseconds}ms with content {Response.Content}",
            (int)response.StatusCode,
            response.StatusCode,
            elapsed.TotalMilliseconds.ToString("F1"), content
        );
    }

    public ValueTask LogRequestFailedAsync(object? context, HttpRequestMessage request, HttpResponseMessage? response,
        Exception exception, TimeSpan elapsed, CancellationToken cancellationToken = new CancellationToken())
    {
        _logger.LogError(
            exception,
            "Request towards '{Request.Host}{Request.Path}' failed after {Response.ElapsedMilliseconds}ms",
            request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
            request.RequestUri!.PathAndQuery,
            elapsed.TotalMilliseconds.ToString("F1"));
        return ValueTask.CompletedTask;
    }

    public object? LogRequestStart(HttpRequestMessage request)
    {
        var content = request.Content?.ReadAsStringAsync().Result;
        _logger.LogInformation(
            "Sending '{Request.Method}' to '{Request.Host}{Request.Path}' with content {Request.Content}",
            request.Method,
            request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
            request.RequestUri!.PathAndQuery,
            content);
        return null;
    }

    public void LogRequestStop(object? context, HttpRequestMessage request, HttpResponseMessage response,
        TimeSpan elapsed)
    {
        var content = response.Content.ReadAsStringAsync().Result;
        _logger.LogInformation(
            "Received '{Response.StatusCodeInt} {Response.StatusCodeString}' after {Response.ElapsedMilliseconds}ms with content {Response.Content}",
            (int)response.StatusCode,
            response.StatusCode,
            elapsed.TotalMilliseconds.ToString("F1"), content
        );
    }

    public void LogRequestFailed(object? context, HttpRequestMessage request, HttpResponseMessage? response,
        Exception exception,
        TimeSpan elapsed)
    {
        _logger.LogError(
            exception,
            "Request towards '{Request.Host}{Request.Path}' failed after {Response.ElapsedMilliseconds}ms",
            request.RequestUri?.GetComponents(UriComponents.SchemeAndServer, UriFormat.Unescaped),
            request.RequestUri!.PathAndQuery,
            elapsed.TotalMilliseconds.ToString("F1"));
    }
}