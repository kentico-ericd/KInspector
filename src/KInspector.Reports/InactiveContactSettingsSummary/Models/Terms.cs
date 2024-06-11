using KInspector.Core.Models;

namespace KInspector.Reports.InactiveContactSettingsSummary.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class Summaries
    {

        public Term? Information { get; set; }
    }

    public class TableTitles
    {
        public Term? SettingsTableName { get; set; }
    }
}
