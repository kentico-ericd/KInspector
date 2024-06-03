using KInspector.Core.Models;

namespace KInspector.Reports.OnlineMarketingTableAnalysis.Models
{
    public class Terms
    {
        public Term? Good { get; set; }

        public Term? IssuesFound { get; set; }

        public Term? ActivityIssues { get; set; }

        public Term? ContactIssues { get; set; }

        public Term? ContactGroupIssues { get; set; }

        public Term? ScoringRuleIssues { get; set; }
    }
}
