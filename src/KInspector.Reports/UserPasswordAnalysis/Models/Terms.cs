using KInspector.Core.Models;

namespace KInspector.Reports.UserPasswordAnalysis.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitlesTerms? TableTitles { get; set; }
    }

    public class Summaries
    {
        public Term? Error { get; set; }

        public Term? Good { get; set; }
    }

    public class TableTitlesTerms
    {
        public Term? EmptyPasswords { get; set; }

        public Term? PlaintextPasswords { get; set; }
    }
}