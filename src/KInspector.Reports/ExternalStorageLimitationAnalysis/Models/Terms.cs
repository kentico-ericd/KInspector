using KInspector.Core.Models;

namespace KInspector.Reports.ExternalStorageLimitationAnalysis.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class Summaries
    {
        public Term? Good { get; set; }

        public Term? Error { get; set; }
    }

    public class TableTitles
    {
        public Term? TablesOverLimit { get; set; }
    }
}