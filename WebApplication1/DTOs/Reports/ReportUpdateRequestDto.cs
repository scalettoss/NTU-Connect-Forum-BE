namespace ForumBE.DTOs.Reports
{
    public class ReportUpdateRequestDto
    {
        public string Reason { get; set; }
    }

    public class HandelReportRequestDto
    {
        public int ReportId { get; set; }
        public string Status { get; set; }

    }
}
