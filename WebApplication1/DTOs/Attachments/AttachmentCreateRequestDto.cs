namespace ForumBE.DTOs.Attachments
{
    public class AttachmentCreateRequestDto
    {
        public int? PostId { get; set; }
        public int? CommentId { get; set; }
        public List<IFormFile>? Files { get; set; }
    }
}
