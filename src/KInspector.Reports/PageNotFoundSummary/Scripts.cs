namespace KInspector.Reports.PageNotFoundSummary
{
    public static class Scripts
    {
        public static string BaseDirectory => $"{nameof(PageNotFoundSummary)}/Scripts";

        public static string GetPageNotFoundEventLogEntries => $"{BaseDirectory}/{nameof(GetPageNotFoundEventLogEntries)}.sql";
    }
}
