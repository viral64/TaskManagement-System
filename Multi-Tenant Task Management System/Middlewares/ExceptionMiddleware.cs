using System.Net;
using System.Text.Json;

namespace Multi_Tenant_Task_Management_System.Middlewares
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
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
                _logger.LogError(ex, "Unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;

            // Customize known exceptions
            if (ex is UnauthorizedAccessException)
                code = HttpStatusCode.Unauthorized;
            else if (ex is ArgumentException)
                code = HttpStatusCode.BadRequest;

            var response = new
            {
                StatusCode = (int)code,
                Message = ex.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
