﻿using KInspector.Core.Models;

namespace KInspector.Actions.DisableWebFarmServers.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class TableTitles
    {
        public Term? WebFarmServers { get; set; }
    }

    public class Summaries
    {
        public Term? InvalidOptions { get; set; }

        public Term? ListSummary { get; set; }

        public Term? ServerDisabled { get; set; }
    }
}
