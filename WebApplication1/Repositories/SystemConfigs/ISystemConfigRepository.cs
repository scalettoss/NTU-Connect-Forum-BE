using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.SystemConfigs
{
    public interface ISystemConfigRepository : IGenericRepository<SystemConfig>
    {
        Task<SystemConfig> GetByKeyAsync(string key);
    }
}
