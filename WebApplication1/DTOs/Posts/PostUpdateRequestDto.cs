namespace ForumBE.DTOs.Posts
{
    public class PostUpdateRequestDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
    }

    public class PostUpdateByAdminRequestDto
    {
        public string? Title { get; set; }
        public string? Content { get; set; }
        public string? Status { get; set; }
    }
}
