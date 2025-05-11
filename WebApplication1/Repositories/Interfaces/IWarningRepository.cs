using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Interfaces
{
    public interface IWarningRepository : IGenericRepository<Warning>
    {
        Task<int> GetCountByUserId(int userId);
    }
}
