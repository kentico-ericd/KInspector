namespace KInspector.Reports.PageNotFoundSummary.Models
{
    public class CmsNotFoundEvent
    {
        public int Count { get; set; }

        public string? EventUrl { get; set; }

        public DateTime FirstOccurrence { get; set; }

        public string? Referrer { get; set; }
    }
}
