using AutoMapper;
using ForumBE.DTOs.Warnings;
using ForumBE.Helpers;
using ForumBE.Models;
using ForumBE.Repositories.Interfaces;
using ForumBE.Repositories.Users;
using ForumBE.Services.Interfaces;

namespace ForumBE.Services.Implementations
{
    public class WarningService : IWarningService
    {
        private readonly IWarningRepository _warningRepository;
        private readonly IMapper _mapper;
        private readonly ClaimContext _userContextService;
        private readonly IUserRepository _userRepository;
        public WarningService(IWarningRepository warningRepository, IMapper mapper, ClaimContext userContextService, IUserRepository userRepository)
        {
            _warningRepository = warningRepository;
            _mapper = mapper;
            _userContextService = userContextService;
            _userRepository = userRepository;
        }
        public async Task<List<WarningResponseDto>> GetAllWarningsAsync()
        {
            var warnings = await _warningRepository.GetAllAsync();
            var warningsDto = _mapper.Map<List<WarningResponseDto>>(warnings);
            return warningsDto;
        }

        public async Task<int> GetWarningsCountByUserId(int userId)
        {
            var count = await _warningRepository.GetCountByUserId(userId);
            return count;
        }

        public async Task<bool> CreateWarningAsync(WarningCreateRequestDto input)
        {
            var userId = _userContextService.GetUserId();
            var existingUser = await _userRepository.GetByIdAsync(input.UserId);
            if (existingUser == null)
            {
                throw new Exception("User not found");
            }
            var warning = _mapper.Map<Warning>(input);
            warning.IssuedBy = userId;
            warning.CreatedAt = DateTime.Now;

            await _warningRepository.AddAsync(warning);
            return true;
        }
        public async Task<bool> UpdateWarningAsync(int id, WarningUpdateRequestDto input)
        {
            var warning = await _warningRepository.GetByIdAsync(id);
            if (warning == null)
            {
                throw new Exception("Warning not found");
            }
            _mapper.Map(input, warning);
            warning.UpdatedAt = DateTime.Now;
            await _warningRepository.UpdateAsync(warning);

            return true;
        }

        public async Task<bool> DeleteWarningAsync(int id)
        {
            var warning = await _warningRepository.GetByIdAsync(id);
            if (warning == null)
            {
                throw new Exception("Warning not found");
            }
            await _warningRepository.DeleteAsync(warning);

            return true;
        }

        

        
    }
}
