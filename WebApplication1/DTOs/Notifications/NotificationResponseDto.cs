using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Notifications
{
    public class NotificationResponseDto
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }

    }
}
