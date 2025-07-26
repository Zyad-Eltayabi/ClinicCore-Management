using System.Diagnostics;
using System.Text;
using System.Text.Json;
using Serilog.Context;

public class LoggingAsyncMiddleware : IMiddleware
{
    private readonly ILogger<LoggingAsyncMiddleware> _logger;
    private readonly int _maxRequestBodySize;
    private readonly int _maxResponseBodySize;
    private readonly HashSet<string> _sensitiveHeaders;
    private readonly HashSet<string> _sensitiveQueryParams;

    public LoggingAsyncMiddleware(
        ILogger<LoggingAsyncMiddleware> logger,
        int maxRequestBodySize = 1024 * 10, // 10KB default
        int maxResponseBodySize = 1024 * 10) // 10KB default
    {
        _logger = logger;
        _maxRequestBodySize = maxRequestBodySize;
        _maxResponseBodySize = maxResponseBodySize;

        // Define sensitive headers that should not be logged
        _sensitiveHeaders = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "Authorization", "Cookie", "Set-Cookie", "X-API-Key",
            "X-Auth-Token", "Authentication", "Proxy-Authorization"
        };

        // Define sensitive query parameters
        _sensitiveQueryParams = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            "password", "token", "apikey", "secret", "key", "access_token"
        };
    }

    // IMiddleware contract implementation
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        // Generate correlation ID for tracking this specific request
        var correlationId = Guid.NewGuid().ToString();

        // Start timing the request
        var stopwatch = Stopwatch.StartNew();
        var startTime = DateTime.UtcNow;

        // Read and log request information BEFORE next middleware
        var requestBody = await LogRequestAsync(context, correlationId, startTime);

        // Capture the original response body stream
        var originalResponseBodyStream = context.Response.Body;

        Exception? caughtException = null;
        using var responseBodyStream = new MemoryStream();


        // Replace response body stream to capture response
        context.Response.Body = responseBodyStream;

        // Execute the next middleware in the pipeline
        await next(context);

        // Stop timing
        stopwatch.Stop();
        var endTime = DateTime.UtcNow;

        // Read response body BEFORE any stream operations
        string? responseBody = null;
        if (responseBodyStream.Length > 0)
        {
            responseBodyStream.Position = 0;
            responseBody = await ReadResponseBodyFromStreamAsync(responseBodyStream, context.Response.ContentType);
        }

        // Log response information with the captured response body
        await LogResponseAsync(context, correlationId, startTime, endTime,
            stopwatch.ElapsedMilliseconds, responseBody,
            caughtException, requestBody);

        // Copy captured response back to original stream
        if (responseBodyStream.Length > 0)
        {
            responseBodyStream.Position = 0;
            await responseBodyStream.CopyToAsync(originalResponseBodyStream);
        }

        // Restore original response body stream
        context.Response.Body = originalResponseBodyStream;
    }

    private async Task<string?> LogRequestAsync(HttpContext context, string correlationId, DateTime startTime)
    {
        var request = context.Request;

        // Read request body BEFORE it's consumed by model binding
        var requestBody = await ReadRequestBodyAsync(request);

        // Capture basic request information
        var requestInfo = new
        {
            CorrelationId = correlationId,
            Timestamp = startTime,
            request.Method,
            Url = $"{request.Scheme}://{request.Host}{request.Path}{request.QueryString}",
            Path = request.Path.Value,
            QueryString = request.QueryString.Value,
            request.Protocol,
            request.ContentType,
            request.ContentLength,

            // Client information
            ClientIP = GetClientIpAddress(context),
            UserAgent = request.Headers["User-Agent"].FirstOrDefault(),

            // User information (if authenticated)
            UserId = context.User?.Identity?.Name,
            IsAuthenticated = context.User?.Identity?.IsAuthenticated ?? false,

            // Filtered headers (excluding sensitive ones)
            Headers = FilterSensitiveHeaders(request.Headers),

            // Filtered query parameters
            QueryParameters = FilterSensitiveQueryParams(request.Query),

            // Request body
            Body = FormatBodyForLogging(requestBody, request.ContentType)
        };

        // Log the request information
        using (LogContext.PushProperty("UniqueId", correlationId))
        {
            _logger.LogInformation("HTTP Request: {@RequestInfo}", requestInfo);
        }

        // Store correlation ID in HttpContext for later use
        context.Items["CorrelationId"] = correlationId;

        return requestBody;
    }

    private async Task LogResponseAsync(HttpContext context, string correlationId,
        DateTime startTime, DateTime endTime, long durationMs,
        string? responseBody, Exception? exception,
        string? requestBody)
    {
        var response = context.Response;

        // Capture response information
        var responseInfo = new
        {
            CorrelationId = correlationId,
            RequestTimestamp = startTime,
            ResponseTimestamp = endTime,
            DurationMs = durationMs,
            response.StatusCode,
            StatusDescription = GetStatusDescription(response.StatusCode),
            response.ContentType,
            ContentLength = response.ContentLength ?? responseBody?.Length ?? 0,

            // Response headers (filtered)
            Headers = FilterSensitiveHeaders(response.Headers.ToDictionary(h => h.Key, h => h.Value.AsEnumerable())),

            // Exception information (if any)
            Exception = exception != null
                ? new
                {
                    Type = exception.GetType().Name, exception.Message
                }
                : null,

            // Performance classification
            PerformanceCategory = ClassifyPerformance(durationMs),

            // Response body
            Body = FormatBodyForLogging(responseBody, response.ContentType),

            // Request body for correlation
            RequestBody = FormatBodyForLogging(requestBody, context.Request.ContentType)
        };

        // Log with appropriate level based on status code
        var logLevel = GetLogLevel(response.StatusCode, exception);
        using (LogContext.PushProperty("UniqueId", correlationId))
        {
            _logger.Log(logLevel, "HTTP Response: {@ResponseInfo}", responseInfo);
        }
    }

    private string GetClientIpAddress(HttpContext context)
    {
        // Check for forwarded IP first (common in load balancer scenarios)
        var forwardedFor = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrEmpty(forwardedFor))
            // Take the first IP if multiple are present
            return forwardedFor.Split(',')[0].Trim();

        // Check for real IP header
        var realIp = context.Request.Headers["X-Real-IP"].FirstOrDefault();
        if (!string.IsNullOrEmpty(realIp)) return realIp;

        // Fall back to connection remote IP
        return context.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
    }

    private Dictionary<string, object> FilterSensitiveHeaders(IHeaderDictionary headers)
    {
        // Create a filtered dictionary of headers, excluding sensitive ones
        return headers
            .Where(h => !_sensitiveHeaders.Contains(h.Key))
            .ToDictionary(h => h.Key, h => (object)h.Value.ToString());
    }

    private Dictionary<string, object> FilterSensitiveHeaders(Dictionary<string, IEnumerable<string>> headers)
    {
        // Overload for response headers
        return headers
            .Where(h => !_sensitiveHeaders.Contains(h.Key))
            .ToDictionary(h => h.Key, h => (object)string.Join(", ", h.Value));
    }

    private Dictionary<string, string> FilterSensitiveQueryParams(IQueryCollection queryParams)
    {
        // Filter out sensitive query parameters
        return queryParams
            .Where(q => !_sensitiveQueryParams.Contains(q.Key))
            .ToDictionary(q => q.Key, q => q.Value.ToString());
    }

    private async Task<string?> ReadRequestBodyAsync(HttpRequest request)
    {
        // Only read body for POST, PUT, PATCH requests
        if (!HttpMethods.IsPost(request.Method) &&
            !HttpMethods.IsPut(request.Method) &&
            !HttpMethods.IsPatch(request.Method))
            return null;

        // Check content length
        if (request.ContentLength == null || request.ContentLength == 0) return null;

        // Don't log if body is too large
        if (request.ContentLength > _maxRequestBodySize) return $"[Body too large: {request.ContentLength} bytes]";

        try
        {
            // Enable buffering to allow multiple reads
            request.EnableBuffering();

            // Reset position to beginning
            request.Body.Position = 0;

            // Read the body
            using var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            // Reset position for next middleware/model binding
            request.Body.Position = 0;

            return body;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error reading request body");
            return $"[Error reading body: {ex.Message}]";
        }
    }

    private async Task<string?> ReadResponseBodyFromStreamAsync(MemoryStream responseStream, string? contentType)
    {
        // Don't read if stream is empty
        if (responseStream.Length == 0) return null;

        // Don't log if body is too large
        if (responseStream.Length > _maxResponseBodySize) return $"[Response too large: {responseStream.Length} bytes]";

        // Only read readable content types
        if (!IsReadableContentType(contentType)) return $"[Non-readable content: {contentType}]";

        try
        {
            // Store current position
            var originalPosition = responseStream.Position;

            // Reset position to beginning to read all content
            responseStream.Position = 0;

            // Read the response body
            using var reader = new StreamReader(responseStream, Encoding.UTF8, leaveOpen: true);
            var body = await reader.ReadToEndAsync();

            // Restore position (important for copying back to original stream)
            responseStream.Position = originalPosition;

            return body;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error reading response body");
            return $"[Error reading response body: {ex.Message}]";
        }
    }

    private string? FormatBodyForLogging(string? body, string? contentType)
    {
        if (string.IsNullOrWhiteSpace(body)) return null;

        if (IsJsonContent(contentType))
        {
            try
            {
                using var jsonDoc = JsonDocument.Parse(body);
                return JsonSerializer.Serialize(jsonDoc.RootElement, new JsonSerializerOptions
                {
                    WriteIndented = false
                });
            }
            catch (JsonException)
            {
                return "[Invalid JSON]";
            }
        }

        if (contentType?.Contains("application/x-www-form-urlencoded", StringComparison.OrdinalIgnoreCase) == true)
            return Uri.UnescapeDataString(body);

        if (contentType?.Contains("multipart/form-data", StringComparison.OrdinalIgnoreCase) == true)
            return "[Multipart/form-data content omitted]";

        return body.Length > _maxRequestBodySize
            ? $"[Truncated Body: {body.Substring(0, _maxRequestBodySize)}...]"
            : body;
    }


    private bool IsJsonContent(string? contentType)
    {
        // Check if content type indicates JSON
        return !string.IsNullOrEmpty(contentType) &&
               (contentType.Contains("application/json", StringComparison.OrdinalIgnoreCase) ||
                contentType.Contains("text/json", StringComparison.OrdinalIgnoreCase));
    }

    private bool IsReadableContentType(string? contentType)
    {
        if (string.IsNullOrEmpty(contentType)) return false;

        var readableTypes = new[]
        {
            "application/json",
            "application/xml",
            "text/xml",
            "text/plain",
            "text/html",
            "application/x-www-form-urlencoded"
        };

        return readableTypes.Any(type =>
            contentType.Contains(type, StringComparison.OrdinalIgnoreCase));
    }

    private string GetStatusDescription(int statusCode)
    {
        // Provide human-readable status descriptions
        return statusCode switch
        {
            200 => "OK",
            201 => "Created",
            204 => "No Content",
            400 => "Bad Request",
            401 => "Unauthorized",
            403 => "Forbidden",
            404 => "Not Found",
            409 => "Conflict",
            422 => "Unprocessable Entity",
            500 => "Internal Server Error",
            502 => "Bad Gateway",
            503 => "Service Unavailable",
            _ => $"HTTP {statusCode}"
        };
    }

    private string ClassifyPerformance(long durationMs)
    {
        // Classify request performance
        return durationMs switch
        {
            < 100 => "Fast",
            < 500 => "Normal",
            < 1000 => "Slow",
            < 2000 => "Very Slow",
            _ => "Critical"
        };
    }

    private LogLevel GetLogLevel(int statusCode, Exception? exception)
    {
        // Determine appropriate log level based on response
        if (exception != null) return LogLevel.Error;

        return statusCode switch
        {
            >= 500 => LogLevel.Critical,
            >= 400 => LogLevel.Error,
            _ => LogLevel.Information
        };
    }
}