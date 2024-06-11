using KInspector.Core.Models;

namespace KInspector.Reports.PageNotFoundSummary.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class Summaries
    {
        public Term? Good { get; set; }

        public Term? IssuesFound { get; set; }
    }

    public class TableTitles
    {
        public Term? NotFoundEventsTable { get; set; }
    }
}
