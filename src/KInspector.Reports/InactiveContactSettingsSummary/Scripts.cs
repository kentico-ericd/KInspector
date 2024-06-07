namespace KInspector.Reports.InactiveContactSettingsSummary
{
    public static class Scripts
    {
        public static string BaseDirectory => $"{nameof(InactiveContactSettingsSummary)}/Scripts";

        public static string GetInactiveContactDeletionSettings => $"{BaseDirectory}/{nameof(GetInactiveContactDeletionSettings)}.sql";
    }
}
