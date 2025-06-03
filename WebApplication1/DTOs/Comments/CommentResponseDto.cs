using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Comments
{
    public class CommentResponseDto
    {
        public int CommentId { get; set; }
        public string FullName { get; set; }
        public int UserId { get; set; }
        public string Content { get; set; }
        public int LikeCount { get; set; }
        public string AvatarUrl { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDeleted { get; set; }
        public List<string> FileTypes { get; set; }
        public List<string> FileUrls { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<CommentResponseDto> Replies { get; set; } = new();
    }
}
