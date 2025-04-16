using ForumBE.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Attachments
{
    public class AttachmentResponseDto
    {
        public int AttachmentId { get; set; }

        public int? CategoryId { get; set; }

        public int? PostId { get; set; }

        public int? CommentId { get; set; }

        public int UserId { get; set; }

        public string FileUrl { get; set; }

        public string FileType { get; set; }

        public string FileSize { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatdedAt { get; set; }
    }
}
