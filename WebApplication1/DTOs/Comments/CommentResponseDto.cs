using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Comments
{
    public class CommentResponseDto
    {
        public int CommentId { get; set; }

        public int PostId { get; set; }

        public int UserId { get; set; }
        public string Content { get; set; }

        public DateTime? CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
