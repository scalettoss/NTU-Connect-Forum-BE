using ForumBE.Services.User;
using Microsoft.AspNetCore.Authorization;
using System.Text.Json;

namespace ForumBE.Middlewares
{
    public class HandlingAuthentication
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<HandlingAuthentication> _logger;

        public HandlingAuthentication(RequestDelegate next, ILogger<HandlingAuthentication> logger)
        {
            _next = next;
            _logger = logger;
        }


        public async Task InvokeAsync(HttpContext context) 
        {
            var endpoint = context.GetEndpoint();
            if (endpoint?.Metadata?.GetMetadata<IAllowAnonymous>() != null)
            {
                await _next(context);
                return;
            }


            if (context.User.Identity?.IsAuthenticated != true)
            {
                _logger.LogWarning("Unauthorized access attempt to {Path}", context.Request.Path);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 401,
                    message = "Vui lòng đăng nhập"
                }));
                return;
            }

            var userService = context.RequestServices.GetRequiredService<IUserService>();
            var userIdClaim = context.User.Claims.FirstOrDefault(c => c.Type == "UserId")?.Value;

            if (!int.TryParse(userIdClaim, out int userId))
            {
                _logger.LogError("Invalid UserId claim.");
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 400,
                    message = "Invalid UserId claim."
                }));
                return;
            }

            var user = await userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                _logger.LogWarning("User not found: UserId = {UserId}", userId);
                context.Response.StatusCode = StatusCodes.Status404NotFound;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 404,
                    message = "Không tìm thấy người dùng"
                }));
                return;
            }

            if (!user.IsActive)
            {
                _logger.LogWarning("Banned account tried to access: UserId = {UserId}", userId);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 403,
                    message = "Tài khoản đã bị khóa"
                }));
                return;
            }

            if (user.IsDeleted)
            {
                _logger.LogWarning("Account has been removed: UserId = {UserId}", userId);
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(JsonSerializer.Serialize(new
                {
                    status = 400,
                    message = "Tài khoản đã bị xóa"
                }));
                return;
            }

            await _next(context);
        }
    }
}
