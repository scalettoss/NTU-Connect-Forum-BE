
using ForumBE.DTOs.Exception;
using System.Security.Claims;

namespace ForumBE.Helpers
{
    public class ClaimContext
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimContext(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int GetUserId()
        {
            var userIdClaim = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "UserId")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                return 0;
            }

            if (!int.TryParse(userIdClaim, out var userId))
            {
                throw new HandleException("Invalid UserId in token!", 401);
            }
            return userId;
        }

        public string GetUserEmail()
        {
            var userEmailClaim = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "Email")?.Value;

            if (string.IsNullOrEmpty(userEmailClaim))
            {
                throw new HandleException("User is not authorized!", 401);
            }
            return userEmailClaim;
        }
        public string GetUserRoleName()
        {
            var userRoleClaim = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == "RoleName")?.Value;

            if (string.IsNullOrEmpty(userRoleClaim))
            {
                throw new HandleException("User is not authorized!", 401);
            }
            return userRoleClaim;
        }
        public bool IsInRole(string roleName)
        {
            var roleClaim = _httpContextAccessor.HttpContext?.User.Claims
                .FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (string.IsNullOrEmpty(roleClaim))
            {
                return false;
            }
            return roleClaim.Equals(roleName, StringComparison.OrdinalIgnoreCase);
        }
    }

}
