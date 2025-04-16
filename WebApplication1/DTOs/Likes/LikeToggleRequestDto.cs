namespace ForumBE.DTOs.Likes
{
    public class LikeToggleRequestDto
    {
        public int? PostId { get; set; } = null;
        public int? CommentId { get; set; } = null;
    }
}
