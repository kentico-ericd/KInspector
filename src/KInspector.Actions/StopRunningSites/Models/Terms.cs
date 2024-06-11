using KInspector.Core.Models;

namespace KInspector.Actions.StopRunningSites.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class TableTitles
    {
        public Term? Sites { get; set; }
    }

    public class Summaries
    {
        public Term? InvalidOptions { get; set; }

        public Term? ListSummary { get; set; }

        public Term? SiteStopped { get; set; }
    }
}
