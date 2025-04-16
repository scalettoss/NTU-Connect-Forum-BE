using ForumBE.Models;

namespace ForumBE.Repositories.Interfaces
{
    public interface IWarningRepository : IGenericRepository<Warning>
    {
        Task<int> GetCountByUserId(int userId);
    }
}
