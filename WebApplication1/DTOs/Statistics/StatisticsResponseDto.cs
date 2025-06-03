namespace ForumBE.DTOs.Statistics
{
    public class StatisticsResponseDto
    {
        public int TotalUsers { get; set; }
        public int TotalPosts { get; set; }
        public int TotalComments { get; set; }
        public int TotalCategories { get; set; }
        public int TotalReports { get; set; }
        public int TotalUploadFiles { get; set; }
    }

    public class LatestActivityResponseDto
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public string CreatedAt { get; set; }
    }
}
