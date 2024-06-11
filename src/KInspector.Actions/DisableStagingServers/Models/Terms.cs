using KInspector.Core.Models;

namespace KInspector.Actions.DisableStagingServers.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class TableTitles
    {
        public Term? StagingServers { get; set; }
    }

    public class Summaries
    {
        public Term? InvalidOptions { get; set; }

        public Term? ListSummary { get; set; }

        public Term? ServerDisabled { get; set; }
    }
}
