using ForumBE.DTOs.Paginations;
using ForumBE.DTOs.Posts;
using ForumBE.DTOs.Posts.ForumBE.DTOs.Posts;

namespace ForumBE.Services.Post
{
    public interface IPostService
    {
        Task<PaginationData<PostResponseDto>> GetAllPostsAsync(PaginationParams input);
        Task<PaginationData<PostResponseDto>> GetAllPostByCategoryIdAsync(int categoryId, PaginationParams input);
        Task<PostResponseDto> GetPostByIdAsync(int postId);
        Task<bool> CreatePostAsync(PostCreateRequestDto request);
        Task<bool> UpdatePostAsync(int postId, PostUpdateRequest request);
        Task<bool> DeletePostAsync(int postId);
    }
}
