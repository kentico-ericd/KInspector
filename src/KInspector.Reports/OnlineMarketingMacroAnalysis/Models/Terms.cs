using KInspector.Core.Models;

namespace KInspector.Reports.OnlineMarketingMacroAnalysis.Models
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
        public Term? ContactGroupTable { get; set; }

        public Term? AutomationTriggerTable { get; set; }

        public Term? ScoreRuleTable { get; set; }
    }
}