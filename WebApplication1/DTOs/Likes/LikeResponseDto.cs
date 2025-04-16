using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Likes
{
    public class LikeResponseDto
    {
        public int LikeId { get; set; }
        public int UserId { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public bool IsLike { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
