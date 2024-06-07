namespace KInspector.Reports.EventLogErrorSummary
{
    public static class Scripts
    {
        public static string BaseDirectory => $"{nameof(EventLogErrorSummary)}/Scripts";

        public static string GetEventLogErrors => $"{BaseDirectory}/{nameof(GetEventLogErrors)}.sql";
    }
}
