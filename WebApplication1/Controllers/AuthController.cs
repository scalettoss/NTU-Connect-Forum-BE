using ForumBE.Auth;
using ForumBE.DTOs.Auth.Login;
using ForumBE.DTOs.Auth.Register;
using ForumBE.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ForumBE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        public AuthController(AuthService authService)
        {
            _authService = authService;   
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ResponseBase> RegisterUser([FromBody] RegisterRequestDto input)
        {
            var isCreated = await _authService.RegisterUserAsync(input);
            if(!isCreated)
            {
                return ResponseBase.Fail("User created failed", 400);
            }
            return ResponseBase.Success("User created successfully");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<ResponseBase> LoginUser([FromBody] LoginRequestDto input)
        {
            var result = await _authService.Authenticate(input);
            return ResponseBase.Success(result);
        }

    }
}
