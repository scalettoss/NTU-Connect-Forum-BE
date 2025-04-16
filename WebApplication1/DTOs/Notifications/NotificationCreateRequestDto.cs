namespace ForumBE.DTOs.Notifications
{
    public class NotificationCreateRequestDto
    {
        public string Message { get; set; }
        public string Type { get; set; }
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
    }
}
