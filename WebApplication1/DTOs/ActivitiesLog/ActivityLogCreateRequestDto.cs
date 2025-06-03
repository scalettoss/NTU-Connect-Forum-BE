namespace ForumBE.DTOs.ActivitiesLog
{
    public class ActivityLogCreateRequestDto
    {
        public string Action { get; set; }
        public string Description { get; set; }
        public string Module { get; set; }
    }
}
