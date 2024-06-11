using KInspector.Core.Models;

namespace KInspector.Reports.EventLogErrorSummary.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class Summaries
    {
        public Term? Good { get; set; }

        public Term? Information { get; set; }
    }

    public class TableTitles
    {
        public Term? EventLogTableName { get; set; }
    }
}