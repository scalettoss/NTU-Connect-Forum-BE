using AutoMapper;
using ForumBE.DTOs.Auth.Login;
using ForumBE.DTOs.Auth.Register;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.UserProflies;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.ActivitiesLog;
using ForumBE.Repositories.Interfaces;
using ForumBE.Repositories.Notifications;
using ForumBE.Repositories.Users;
using ForumBE.Services.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ForumBE.Services.Auth
{
    public class AuthService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IPasswordHasher<Models.User> _passwordHasher;
        private readonly IUserRepository _userRepository;
        private readonly IUserProfileRepository _userProfileRepository;
        private readonly IActivityLogRepository _activityLogRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;

        public AuthService(
            ApplicationDbContext dbContext,
            IConfiguration configuration,
            IUserService userService,
            IPasswordHasher<Models.User> passwordHasher,
            IUserRepository userRepository,
            IMapper mapper,
            IUserProfileRepository userProfileRepository,
            INotificationRepository notificationRepository,
            IActivityLogRepository activityLogRepository)
        {
            _dbContext = dbContext;
            _configuration = configuration;
            _userService = userService;
            _passwordHasher = passwordHasher;
            _userRepository = userRepository;
            _mapper = mapper;
            _userProfileRepository = userProfileRepository;
            _activityLogRepository = activityLogRepository;
            _notificationRepository = notificationRepository;
        }
        public async Task<LoginResponseDto> Authenticate(LoginRequestDto input)
        {
            var userAccount = await _userService.GetUserByEmailAsync(input.Email);

            if (userAccount == null || !_passwordHasher.VerifyHashedPassword(null, userAccount.PasswordHash, input.Password).Equals(PasswordVerificationResult.Success))
            {
                throw new HandleException("Sai tài khoản hoặc mật khẩu", 400);
            }

            var issuer = _configuration["JwtConfig:Issuer"];
            var audience = _configuration["JwtConfig:Audience"];
            var secretKey = _configuration["JwtConfig:SecretKey"];
            var tokenValidMins = _configuration.GetValue<int>("JwtConfig:AccessTokenExpirationM");
            var tokenTimeStamp = DateTime.Now.AddMinutes(tokenValidMins);
            var roleName = userAccount.RoleName;
            var userId = userAccount.UserId;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
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

            var log = new ActivityLog
            {
                UserId = userId,
                Action = ConstantString.LoginAction,
                Description = $"Đăng nhập thành công với email {input.Email}",
                Module = "AuthModule",
                CreatedAt = DateTime.Now,
                IsDeleted = true,
            };
            await _activityLogRepository.AddAsync(log);
            return new LoginResponseDto
            {
                AccessToken = accessToken,
                Expires = (int)tokenTimeStamp.Subtract(DateTime.Now).TotalMinutes,
            };

        }

        public async Task<bool> RegisterUserAsync(RegisterRequestDto input)
        {

            if (!Regex.IsMatch(input.Email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                throw new HandleException("Email không đúng định dạng", 400);
            }

            if (input.Password.Length < 6)
            {
                throw new HandleException("Mật khẩu tối thiểu 6 kí tự", 400);
            }

            var userExist = await _userRepository.GetByEmailAsync(input.Email);

            if (userExist != null)
            {
                throw new HandleException("Email đã được đăng kí", 409);
            }

            if (input.ConfirmPassword != input.Password)
            {
                throw new HandleException("Mật khẩu không khớp", 400);
            }

            var hashedPassword = _passwordHasher.HashPassword(null, input.Password);

            var user = new Models.User
            {
                Email = input.Email,
                PasswordHash = hashedPassword,
                FirstName = input.FirstName,
                LastName = input.LastName,
                CreatedAt = DateTime.Now,
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
                AvatarUrl = "/images/default_avatar.png",
                Bio = null,
                DateOfBirth = null,
                Gender = null,
                IsProfilePublic = true,
                PhoneNumber = null

            };
            var userProflie = _mapper.Map<UserProfile>(userProfileDto);
            await _userProfileRepository.AddAsync(userProflie);

            var notifications = new Notification
            {
                UserId = alreadyUser.UserId,
                SenderId = null,
                Type = "System",
                Message = $"Chào mừng người dùng {alreadyUser.FirstName + " " + alreadyUser.LastName} đến với diễn đàn",
                IsRead = false,
                CreatedAt = DateTime.Now,
                IsDeleted = false
            };
            await _notificationRepository.AddAsync(notifications);

            var log = new ActivityLog
            {
                UserId = alreadyUser.UserId,
                Action = ConstantString.RegisterAction,
                Description = $"Đăng kí tài khoản thành công với email {input.Email}",
                Module = "AuthModule",
                CreatedAt = DateTime.Now,
                IsDeleted = false,
            };
            await _activityLogRepository.AddAsync(log);
            return true;
        }
    }
}
