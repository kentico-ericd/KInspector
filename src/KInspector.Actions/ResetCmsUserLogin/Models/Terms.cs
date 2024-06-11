using KInspector.Core.Models;

namespace KInspector.Actions.ResetCmsUserLogin.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class TableTitles
    {
        public Term? GlobalAdmins { get; set; }
    }

    public class Summaries
    {
        public Term? InvalidOptions { get; set; }

        public Term? ListSummary { get; set; }

        public Term? UserReset { get; set; }
    }
}
