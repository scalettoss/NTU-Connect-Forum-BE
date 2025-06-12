using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;

namespace ForumBE.Services.IPost
{
    public interface IPostService
    {
        Task<PagedList<PostResponseDto>> GetAllPostsAsync(PaginationDto input);
        Task<PagedList<PostResponseDto>> GetAllPostByCategoryAsync(PaginationDto input, string categoryName);
        Task<PagedList<PostAdminResponseDto>> GetAllPostByAdminAsync(PaginationDto input, PostSearchRequestDto condition);
        Task<IEnumerable<PostResponseDto>> GetLatestPostsAsync(string? sortBy);
        Task<PostResponseDto> GetPostByIdAsync(int postId);
        Task<PostAdminResponseDto> GetPostByAdminAsync(int postId);
        Task<PostResponseDto> GetPostBySlugAsync(string slug);
        Task<int> CreatePostAsync(PostCreateRequestDto request);
        Task<bool> UpdatePostAsync(int postId, PostUpdateRequestDto request);
        Task<bool> DeletePostAsync(int postId);
        Task<int> GetUserAuthorByPostAsync(int postId);
        Task<bool> UpdatePostByAdminAsync(PostUpdateByAdminRequestDto request, int id);
    }
}
