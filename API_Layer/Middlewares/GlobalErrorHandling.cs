using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ClinicAPI.Middlewares
{
    public class GlobalErrorHandling : IExceptionHandler
    {
        private readonly ILogger<GlobalErrorHandling> _logger;

        public GlobalErrorHandling(ILogger<GlobalErrorHandling> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogCritical(exception, exception.Message);

            var details = new ProblemDetails()
            {
                Title = "An error occurred while processing your request.",
                Detail = "An unexpected error occurred. Please try again later.",
                Instance = httpContext.Request.Path,
                Type = $"https://httpstatuses.com/{500}",
                Status = (int)StatusCodes.Status500InternalServerError
            };

            var response = JsonSerializer.Serialize(details);
            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsync(response, cancellationToken);
            return true;
        }
    }
}
