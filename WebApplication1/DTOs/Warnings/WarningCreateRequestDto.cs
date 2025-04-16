namespace ForumBE.DTOs.Warnings
{
    public class WarningCreateRequestDto
    {
        public int UserId { get; set; }
        public string Description { get; set; }
        public string Reason { get; set; }
    }
}
