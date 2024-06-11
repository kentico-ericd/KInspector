using KInspector.Core.Models;

namespace KInspector.Reports.ClassTableValidation.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class Summaries {
        public Term? NoIssuesFound { get; set; }

        public Term? CountIssueFound { get; set; }
    }

    public class TableTitles
    {
        public Term? DatabaseTablesWithMissingKenticoClasses { get; set; }

        public Term? KenticoClassesWithMissingDatabaseTables { get; set; }
    }
}