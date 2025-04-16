using AutoMapper;
using ForumBE.DTOs.Exception;
using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;
using ForumBE.DTOs.Users;
using ForumBE.Helpers;
using ForumBE.Repositories.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace ForumBE.Services.User
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IPasswordHasher<Models.User> _passwordHasher;
        private readonly ClaimContext _userContextService;
        private readonly ILogger<UserService> _logger;  

        public UserService(
            IUserRepository userRepository,
            IMapper mapper,
            IPasswordHasher<Models.User> passwordHasher,
            ClaimContext userContextService,
            ILogger<UserService> logger)   
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _passwordHasher = passwordHasher;
            _userContextService = userContextService;
            _logger = logger;
        }

        public async Task<PaginationData<UserResponseDto>> GetAllUserAsync(PaginationParams input)
        {
            _logger.LogInformation("Fetching all users."); 
            var paged = await _userRepository.GetAllUserProfileAsync(input);
            var usersDto = _mapper.Map<IEnumerable<UserResponseDto>>(paged.Items);
            var result = new PaginationData<UserResponseDto>
            {
                Data = usersDto,
                Pagination = new PagedResultDto
                {
                    TotalItems = paged.TotalItems,
                    PageIndex = paged.PageIndex,
                    PageSize = paged.PageSize,
                    TotalPages = input.PageSize > 0 ? (int)Math.Ceiling((double)paged.TotalItems / paged.PageSize) : 0
                }
            };

            return result;
        }

        public async Task<UserResponseDto> GetUserByIdAsync(int id)
        {
            _logger.LogInformation("Fetching user by ID: {UserId}", id); 
            var user = await _userRepository.GetProfileByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id); 
                throw new HandleException("User not found!", 404);
            }
            var userMap = _mapper.Map<UserResponseDto>(user);
            return userMap;
        }

        public async Task<UserResponseDto> GetUserByEmailAsync(string email)
        {
            _logger.LogInformation("Fetching user by email: {Email}", email);
            var user = await _userRepository.GetByEmailAsync(email);
            if (user == null)
            {
                _logger.LogWarning("User with email {Email} not found.", email);
                throw new HandleException("User not found!", 404);
            }
            var userMap = _mapper.Map<UserResponseDto>(user);
            return userMap;
        }

        public async Task<bool> UpdateUserAsync(int id, UserUpdateProfilesRequestDto request)
        {
            _logger.LogInformation("Updating user profile. ID: {UserId}", id);
            var userId = _userContextService.GetUserId();
            if (userId != id)
            {
                _logger.LogWarning("User ID mismatch. Current: {CurrentUserId}, Target: {TargetUserId}", userId, id);
                throw new HandleException("User not authorized", 401);
            }

            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new HandleException("User not found!", 404);
            }

            if (request.AvatarFile != null && request.AvatarFile.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "public", "avatars");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                    _logger.LogInformation("Created uploads folder at {UploadsFolder}", uploadsFolder);
                }

                var fileExtension = Path.GetExtension(request.AvatarFile.FileName);
                var fileName = $"{Guid.NewGuid()}{fileExtension}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await request.AvatarFile.CopyToAsync(stream);
                }

                _logger.LogInformation("Saved avatar file for user {UserId} at {FilePath}", id, filePath);

                request.AvatarUrl = $"/public/avatars/{fileName}";
            }

            _mapper.Map(request, user);
            user.UpdatedAt = DateTime.UtcNow;
            await _userRepository.UpdateAsync(user);
            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            _logger.LogInformation("Deleting user with ID: {UserId}", id);
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found.", id);
                throw new HandleException("User not found!", 404);
            }
            user.IsDeleted = true;
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
                throw new HandleException("User not found!", 404);
            }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, input.OldPassword);
            if (verificationResult != PasswordVerificationResult.Success)
            {
                _logger.LogWarning("Password verification failed for user {Email}.", email);
                throw new HandleException("Password is incorrect.", 401);
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
                throw new HandleException("User not found!", 404);
            }

            user.IsActive = isActive;
            await _userRepository.UpdateAsync(user);
            return true;
        }
    }
}
