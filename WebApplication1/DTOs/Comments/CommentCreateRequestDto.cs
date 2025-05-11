using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Comments
{
    public class CommentCreateRequestDto
    {
        public int PostId { get; set; }
        public int? ReplyTo { get; set; }
        public string Content { get; set; }
    }
}
