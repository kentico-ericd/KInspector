using KInspector.Core.Models;

namespace KInspector.Reports.TemplateLayoutAnalysis.Models
{
    public class Terms
    {
        public Summaries? Summaries { get; set; }

        public TableTitles? TableTitles { get; set; }
    }

    public class Summaries
    {
        public Term? CountIdenticalPageLayoutFound { get; set; }

        public Term? NoIdenticalPageLayoutsFound { get; set; }
    }

    public class TableTitles
    {
        public Term? IdenticalPageLayouts { get; set; }
    }
}