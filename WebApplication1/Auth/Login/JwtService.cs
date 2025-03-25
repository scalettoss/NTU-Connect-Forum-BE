using ForumBE.DTOs.Auth.Login;
using ForumBE.Middlewares.ErrorHandling;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Implementations;
using ForumBE.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ForumBE.Auth.Login
{
    public class JwtService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;
        public JwtService(ApplicationDbContext dbContext, IConfiguration configuration, IUserService userService, IPasswordHasher<User> passwordHasher)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userService = userService;
            _passwordHasher = passwordHasher;
        }
        public async Task<LoginResponseDto> Authenticate(LoginRequestDto input)
        {
            var userAccount = await _userService.GetUserByEmailAsync(input.Email);

            if (userAccount == null)
            {
                throw new HandleException("User Not Found!", 401);
            }

            if (!_passwordHasher.VerifyHashedPassword(null, userAccount.PasswordHash, input.Password).Equals(PasswordVerificationResult.Success))
            {
                throw new HandleException("Invalid username or password.", 401);
            }

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var secretKey = _configuration["JwtConfig:SecretKey"];
            var tokenValidMins = _configuration.GetValue<int>("JwtConfig:AccessTokenExpirationM");
            var tokenTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidMins);
            var roleId = userAccount.RoleId;


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Email, input.Email),
                    new Claim(ClaimTypes.Role, roleId.ToString()),
                }),
                Expires = tokenTimeStamp,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey)),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            var accessToken = tokenHandler.WriteToken(securityToken);

            return new LoginResponseDto 
            {
                Message = "Đăng nhập thành công",
                AccessToken = accessToken,
                Expires = (int)tokenTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
            };
        }
    }
}
