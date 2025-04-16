namespace ForumBE.DTOs.Posts
{
    namespace ForumBE.DTOs.Posts
    {
        public class PostResponseDto
        {
            public int PostId { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string Status { get; set; }
            public bool IsScam { get; set; }
            public bool IsReviewed { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime UpdatedAt { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
        }
    }
}
