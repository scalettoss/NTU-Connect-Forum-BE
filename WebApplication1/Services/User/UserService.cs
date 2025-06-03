using AutoMapper;
using Azure.Core;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.UserProflies;
using ForumBE.DTOs.Users;
using ForumBE.Helpers;
using ForumBE.Repositories.Interfaces;
using ForumBE.Repositories.Users;
using Microsoft.AspNetCore.Identity;

namespace ForumBE.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Models.User> _passwordHasher;
        private readonly ClaimContext _userContextService;
        private readonly ILogger<UserService> _logger;
        private readonly IUserProfileRepository _userProfileRepository;
        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher<Models.User> passwordHasher,
            ClaimContext userContextService,
            ILogger<UserService> logger,
            IUserProfileRepository userProfileRepository)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
            _logger = logger;
            _userProfileRepository = userProfileRepository;
        }
       
        public async Task<PagedList<UserResponseDto>> GetAllUserAsync(PaginationDto input)
        {
            _logger.LogInformation("Fetching user profiles.");
            var users = await _userRepository.GetAllPagesAsync(input);
            return _mapper.Map<PagedList<UserResponseDto>>(users);
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Fetching user by ID: {UserId}", id);
            var user = await _userRepository.GetByUserIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new HandleException("Không tìm thấy người dùng", 404);
            }
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<UserResponseDto> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation("Fetching user by email: {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found.", email);
                throw new HandleException("Không tìm thấy người dùng", 404);
            }
            return _mapper.Map<UserResponseDto>(user);
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new HandleException("Không tìm thấy người dùng", 404);
            }
            user.IsDeleted = true;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ChangePasswordAsync(UserChangePasswordRequestDto input)
        {
            _logger.LogInformation("Changing password for user.");
            var email = _userContextService.GetUserEmail();
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found.", email);
                throw new HandleException("Không tìm thấy người dùng", 404);
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, input.OldPassword);
            if (verificationResult != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Password verification failed for user {Email}.", email);
                throw new HandleException("Sai mật khẩu", 401);
            }

            user.PasswordHash = _passwordHasher.HashPassword(user, input.NewPassword);
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> ActiveUser(int id, bool isActive)
        {
            _logger.LogInformation("{Action} user with ID: {UserId}", isActive ? "Activating" : "Deactivating", id);
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new HandleException("Không tìm thấy người dùng", 404);
            }

            user.IsActive = isActive;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<UserProfileResponseDto> GetUserInformationAsync(int id)
        {
            var information = await _userProfileRepository.GetByIdAsync(id);
            if (information == null)
            {
                throw new HandleException("Không tìm thấy thông tin người dùng", 404);
            }
            return _mapper.Map<UserProfileResponseDto>(information);
        }

        public async Task<bool> UpdateUserInformationAsync(UserProfileUpdateRequestDto input, int id)
        {
            var userId = _userContextService.GetUserId();
            if (userId != id)
            {
                throw new HandleException("Người dùng không có quyền thực hiện", 401);
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                throw new HandleException("Không tìm thấy người dùng", 404);
            }
            
            var userProfile = await _userProfileRepository.GetByIdAsync(id);

            if (input.AvatarFile != null && input.AvatarFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileExtension = Path.GetExtension(input.AvatarFile.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await input.AvatarFile.CopyToAsync(stream);
                }

                _logger.LogInformation("Saved avatar file for user {UserId} at {FilePath}", id, filePath);

                var AvatarUrl = $"/avatars/{fileName}";
                userProfile.AvatarUrl = AvatarUrl;
            }
            var userProfileDto = _mapper.Map(input, userProfile);
            await _userProfileRepository.UpdateAsync(userProfileDto);

            if (input.FirstName != null && input.LastName != null)
            {
                user.FirstName = input.FirstName;
                user.LastName = input.LastName;
            }

            user.UpdatedAt = DateTime.Now;
            await _userRepository.UpdateAsync(user);
            return true;

        }

        public async Task<bool> UpdateUserByAdminAsync(UserChangeInforByAdminRequestDto input)
        {
            var user = await _userRepository.GetByIdAsync(input.UserId);
            if (user == null)
            {
                throw new HandleException("Không tìm thấy người dùng", 404);
            }

            if (input.IsActive != null)
            {
                user.IsActive = input.IsActive.Value;
            }

            if (input.RoleId != null)
            {
                user.RoleId = input.RoleId.Value;
            }

            if (input.Email != null)
            {
                var existingUser = await _userRepository.GetByEmailAsync(input.Email);
                if (existingUser != null && existingUser.UserId != input.UserId)
                {
                    throw new HandleException("Email đã tồn tại", 400);
                }
                user.Email = input.Email;
            }

            if (input.Password != null)
            {
                user.PasswordHash = _passwordHasher.HashPassword(user, input.Password);
            }
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<PagedList<UserResponseDto>> GetAllUserByCondition(PaginationDto input, AdvancedUserSearchRequestDto condition)
        {
            var users = await _userRepository.GetByCondition(input, condition);
            var usersDto = _mapper.Map<PagedList<UserResponseDto>>(users);
            _logger.LogInformation("Fetching user profiles with conditions.");
            return usersDto;
        }

        public async Task<bool> AddUserByAdmin(AddUserByAdminRequestDto input)
        {
            var existingUser = await _userRepository.GetByEmailAsync(input.Email);
            if (existingUser != null)
            {
                throw new HandleException("Email đã tồn tại", 400);
            }
            var user = new Models.User
            {
                FirstName = input.FirstName,
                LastName = input.LastName,
                Email = input.Email,
                RoleId = input.RoleId,
                PasswordHash = _passwordHasher.HashPassword(null, input.Password),
                IsActive = input.IsActive,
                CreatedAt = DateTime.Now,
                IsDeleted = false,
            };
            await _userRepository.AddAsync(user);
            return true;
        }
    }
}
