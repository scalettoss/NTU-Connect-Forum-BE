namespace ForumBE.DTOs.Notifications
{
    public class NotificationCreateRequestDto
    {
        // Nguoi nhan thong bao
        public int UserId { get; set; }
        public string Type { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
    }
}
