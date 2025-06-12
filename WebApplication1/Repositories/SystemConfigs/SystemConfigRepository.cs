using ForumBE.Models;
using ForumBE.Repositories.Generics;
using ForumBE.Repositories.ScamDetections;
using Microsoft.EntityFrameworkCore;

namespace ForumBE.Repositories.SystemConfigs
{
    public class SystemConfigRepository : GenericRepository<SystemConfig>, ISystemConfigRepository
    {
        public SystemConfigRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<SystemConfig> GetByKeyAsync(string key)
        {
            var config = await _context.SystemConfigs.FirstOrDefaultAsync(c => c.Key == key);
            return config;
        }
    }
}
