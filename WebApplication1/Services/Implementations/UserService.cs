using AutoMapper;
using ForumBE.DTOs.Users;
using ForumBE.Middlewares.ErrorHandling;
using ForumBE.Models;
using ForumBE.Repositories.Implementations;
using ForumBE.Repositories.Interfaces;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task AddUserAsync(User user)
        {
            await _userRepository.AddUserAsync(user);
        }

        public Task CreateUserAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<GetUserResponseDto>> GetAllUserAsync()
        {
            var userList = await _userRepository.GetAllUserAsync();
            var userListMap = _mapper.Map<IEnumerable<GetUserResponseDto>>(userList);
            return userListMap;
        }

        public async Task<GetUserResponseDto> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            var userMap = _mapper.Map<GetUserResponseDto>(user);
            return userMap;
        }

        public Task<GetUserResponseDto> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateUserAsync(int id, User user)
        {
            throw new NotImplementedException();
        }
    }
}
