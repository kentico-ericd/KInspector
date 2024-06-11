using KInspector.Core.Models;

namespace KInspector.Reports.PageTypeAssignmentAnalysis.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class Summaries
    {
        public Term? Warning { get; set; }

        public Term? NoIssuesFound { get; set; }
    }

    public class TableTitles
    {
        public Term? UnassignedPageTypes { get; set; }
    }
}