using KInspector.Core.Models;

namespace KInspector.Reports.DatabaseTableSizeAnalysis.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class Summaries
    {
        public Term? CheckResultsTableForAnyIssues { get; set; }
    }

    public class TableTitles
    {
        public Term? Top25Results { get; set; }
    }
}