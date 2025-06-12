namespace ForumBE.DTOs.Posts
{
    namespace ForumBE.DTOs.Posts
    {
        public class PostResponseDto
        {
            public int PostId { get; set; }
            public string CategoryName { get; set; }
            public string Title { get; set; }
            public string CategorySlug { get; set; }
            public string PostSlug { get; set; }
            public string Content { get; set; }
            public bool IsScam { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public string FullName { get; set; }
            public string AvatarUrl { get; set; } 
            public int CommentCount { get; set; } 
            public int LikeCount { get; set; }
            public bool IsLiked { get; set; }
            public bool IsBookmark { get; set; }
            public List<string> FileTypes { get; set; }
            public List<string> FileUrls { get; set; }
        }

        public class PostAdminResponseDto
        {
            public int PostId { get; set; }
            public string CategoryName { get; set; }
            public string Title { get; set; }
            public string Status { get; set; }
            public string Content { get; set; }
            public bool IsScam { get; set; }
            public float ConfidenceScore { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime? UpdatedAt { get; set; }
            public string FullName { get; set; }
            public string AvatarUrl { get; set; }
            public List<string> FileTypes { get; set; }
            public List<string> FileUrls { get; set; }
        }
    }
}
