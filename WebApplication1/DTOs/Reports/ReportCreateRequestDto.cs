namespace ForumBE.DTOs.Reports
{
    public class ReportCreateRequestDto
    {
        public int? PostId { get; set; } = null;
        public int? CommentId { get; set; } = null;
        public string Reason { get; set; }
    }
}
