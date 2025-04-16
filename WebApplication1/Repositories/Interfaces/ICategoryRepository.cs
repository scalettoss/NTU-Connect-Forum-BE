namespace ForumBE.Repositories.Interfaces
{
    public interface ICategoryRepository : IGenericRepository<Models.Category>
    {
        Task<bool> IsExistingCategoryName(string name);
    }
}
