using System.ComponentModel.DataAnnotations;

namespace ForumBE.DTOs.Notifications
{
    public class NotificationResponseDto
    {
        public int NotificationId { get; set; }
        public int UserId { get; set; }
        public string Message { get; set; }
        public int? SenderId { get; set; }
        public string SenderName { get; set; }
        public string Type { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //public int? PostId { get; set; }
        public string? PostSlug { get; set; }
        public string? CategorySlug { get; set; }
        //public int? CommentId { get; set; }
    }
}
