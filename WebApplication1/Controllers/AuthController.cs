using ForumBE.Auth.Login;
using ForumBE.Auth.Register;
using ForumBE.DTOs.Auth.Login;
using ForumBE.DTOs.Auth.Register;
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
        private readonly JwtService _jwtService;
        private readonly RegisterService _registerService;
        public AuthController(RegisterService registerService,JwtService jwtService)
        {
            _registerService = registerService;
            _jwtService = jwtService;
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<RegisterResponseDto> RegisterUser([FromBody] RegisterRequestDto input)
        {
            var result = await _registerService.RegisterUserAsync(input);
            return result;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<LoginResponseDto> LoginUser([FromBody] LoginRequestDto input)
        {
            var result = await _jwtService.Authenticate(input);
            return result;
        }

    }
}
