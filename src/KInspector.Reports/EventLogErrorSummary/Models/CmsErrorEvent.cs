namespace KInspector.Reports.EventLogErrorSummary.Models
{
    public class CmsErrorEvent
    {
        public int Count { get; set; }

        public string? EventCode { get; set; }

        public string? EventDescription { get; set; }

        public string? Source { get; set; }

        public DateTime? EventFirstDate { get; set; }

        public DateTime? EventLastDate { get; set; }
    }
}
