using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;
using ForumBE.Models;
using ForumBE.Repositories.Generics;

namespace ForumBE.Repositories.Posts
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<PagedList<Post>> GetAllPagesByCategoryAsync(int categoryId, PaginationDto input);
        Task<PagedList<PostAdminResponseDto>> GetAllByAdmin(PaginationDto input, PostSearchRequestDto condition);
        Task<IEnumerable<Post>> GetLatestPostsAsync(string? sortBy);
        Task<PostAdminResponseDto> GetPostByAdminAsync(int id);
        Task<Post> GetPostBySlugAsync(string slug);
        Task<int> CountByCategoryAsync(int categoryId);
        Task<int> CountPostLikeAsync(int postId);
        Task<int> CountPostCommentAsync(int postId);
        Task<bool> HasUserLikedPostAsync(int postId, int userId);
        Task<int> GetUserAuthorByPostAsync(int postId);
        Task<bool> IsSlugExistsAsync(string slug);
        Task<Post> GetByIdAllStatusAsync(int id);

    }
}
