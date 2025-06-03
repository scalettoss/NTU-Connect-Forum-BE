using ForumBE.DTOs.Likes;

namespace ForumBE.Services.Interfaces
{
    public interface ILikeService
    {
        Task<IEnumerable<LikeResponseDto>> GetAllLikesAsync();
        Task<int> GetLikeCountFromPostAsync(int postId);
        Task<int> GetLikeCountFromCommentAsync(int commentId);
        Task<bool> ToggleLikeAsync(LikeToggleRequestDto input);
    }
}
