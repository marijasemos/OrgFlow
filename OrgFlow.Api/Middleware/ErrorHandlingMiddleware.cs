using System.Net;
using System.Text.Json;

namespace OrgFlow.Api.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(
            RequestDelegate next, 
            ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var statusCode = HttpStatusCode.InternalServerError;
            string errorCode = "internal_error";

            switch(ex)
            {
                case ArgumentNullException:
                    statusCode = HttpStatusCode.BadRequest;
                    errorCode = "bad_request";
                    break;
                case KeyNotFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    errorCode = "not_found";
                    break;
                case InvalidOperationException:
                    statusCode = HttpStatusCode.Conflict;
                    errorCode = "invalid_operation";
                    break;
            }

            _logger.LogError(ex, "Unhandled exceprion caught in middleware: {StatusCode}, ErrorCode: {ErrorCode}", (int)statusCode, errorCode);

            var problemDetails = new
            {
                traceId = context.TraceIdentifier,
                errorCode,
                status = (int)statusCode,
                message = ex.Message,
                path = context.Request.Path,
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            var json = JsonSerializer.Serialize(problemDetails);
            await context.Response.WriteAsync(json);
        }
    }
}
