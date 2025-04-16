using AutoMapper;
using ForumBE.DTOs.Auth.Login;
using ForumBE.DTOs.Auth.Register;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.UserProflies;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ForumBE.Auth
{
    public class AuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IMapper _mapper;

        public AuthService(
            ApplicationDbContext dbContext, 
            IConfiguration configuration, 
            IUserService userService, 
            IPasswordHasher<User> passwordHasher, 
            IUserRepository userRepository, 
            IMapper mapper, 
            IUserProfileRepository userProfileRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _mapper = mapper;
            _userProfileRepository = userProfileRepository;
        }
        public async Task<LoginResponseDto> Authenticate(LoginRequestDto input)
        {
            var userAccount = await _userService.GetUserByEmailAsync(input.Email);

            if (userAccount == null)
            {
                throw new HandleException("User Not Found!", 404);
            }

            if (!_passwordHasher.VerifyHashedPassword(null, userAccount.PasswordHash, input.Password).Equals(PasswordVerificationResult.Success))
            {
                throw new HandleException("Password not correct.", 401);
            }

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var secretKey = _configuration["JwtConfig:SecretKey"];
            var tokenValidMins = _configuration.GetValue<int>("JwtConfig:AccessTokenExpirationM");
            var tokenTimeStamp = DateTime.UtcNow.AddMinutes(tokenValidMins);
            var roleName = userAccount.RoleName;
            var userId = userAccount.UserId;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new System.Security.Claims.ClaimsIdentity(new[]
                {
                    new Claim("Email" ,input.Email),
                    new Claim("RoleName", roleName),
                    new Claim("UserId", userId.ToString()),
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
                AccessToken = accessToken,
                Expires = (int)tokenTimeStamp.Subtract(DateTime.UtcNow).TotalSeconds,
            };

        }

        public async Task<bool> RegisterUserAsync(RegisterRequestDto input)
        {

            if (!Regex.IsMatch(input.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new HandleException("Invalid email format.", 400);
            }

            if (input.Password.Length < 6)
            {
                throw new HandleException("Password must be at least 6 characters long.", 400);
            }

            var userExist = await _userRepository.GetByEmailAsync(input.Email);

            if (userExist != null)
            {
                throw new HandleException("Conflict: Email already exists.", 409);
            }

            if (input.ConfirmPassword != input.Password)
            {
                throw new HandleException("ConfirmPassword not match.", 400);
            }

            var hashedPassword = _passwordHasher.HashPassword(null, input.Password);

            var user = new User
            {
                Email = input.Email,
                PasswordHash = hashedPassword,
                FirstName = input.FirstName,
                LastName = input.LastName,
                CreatedAt = DateTime.UtcNow,
                RoleId = 1,
                IsActive = true,
                IsDeleted = false,
            };
            await _userRepository.AddAsync(user);

            var alreadyUser = await _userRepository.GetByEmailAsync(input.Email);

            // sau khi add thành công, tạo user profile
            var userProfileDto = new UserProfileCreateRequestDto
            {
                UserId = alreadyUser.UserId,
                Address = null,
                AvatarUrl = "/images/default_avatar.jpg",
                Bio = null,
                DateOfBirth = null,
                Gender = null,
                IsProfilePublic = true,
                PhoneNumber = null
                
            };
            var userProflie = _mapper.Map<UserProfile>(userProfileDto);
            await _userProfileRepository.AddAsync(userProflie);

            return true;
        }
    }
}
