using KInspector.Core.Models;

namespace KInspector.Actions.DisableSmtpServers.Models
{
    public class Terms
    {
        public TableTitles? TableTitles { get; set; }

        public Summaries? Summaries { get; set; }
    }

    public class TableTitles
    {
        public Term? ServersFromSettingsTable { get; internal set; }

        public Term? ServersFromSmtpTable { get; internal set; }
    }

    public class Summaries
    {
        public Term? InvalidOptions { get; internal set; }

        public Term? ListSummary { get; internal set; }

        public Term? ServerDisabled { get; internal set; }

        public Term? SiteSettingDisabled { get; internal set; }
    }
}
