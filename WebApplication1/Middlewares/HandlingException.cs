using ForumBE.DTOs.Exception;
using System.Text.Json;

namespace ForumBE.Middlewares
{
    public class HandlingException
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandlingException> _logger;

        public HandlingException(RequestDelegate next, ILogger<HandlingException> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError("An unhandled exception occurred: {Message}", ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            var response = new ErrorResponse();
            

            switch (exception)
            {
                case HandleException ex: 
                    context.Response.StatusCode = ex.Status;
                    response.status = ex.Status;
                    response.message = ex.Message;
                    break;

                case ArgumentException:
                    context.Response.StatusCode = StatusCodes.Status400BadRequest;
                    response.status = 1002;
                    response.message = exception.Message;
                    break;

                default:
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    break;
            }

            var jsonResponse = JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(jsonResponse);
        }
        
    }
    public class ErrorResponse
    {
        public int status { get; set; }
        public string message { get; set; }
    }
}
