using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;

namespace ForumBE.Services.Post
{
    public interface IPostService
    {
        Task<PagedList<PostResponseDto>> GetAllPostsAsync(PaginationDto input);
        Task<PagedList<PostResponseDto>> GetAllPostByCategoryAsync(PaginationDto input, string categoryName);
        Task<IEnumerable<PostResponseDto>> GetLatestPostsAsync(string? sortBy);
        Task<PostResponseDto> GetPostByIdAsync(int postId);
        Task<PostResponseDto> GetPostBySlugAsync(string slug);
        Task<int> CreatePostAsync(PostCreateRequestDto request);
        Task<bool> UpdatePostAsync(int postId, PostUpdateRequest request);
        Task<bool> DeletePostAsync(int postId);
    }
}
