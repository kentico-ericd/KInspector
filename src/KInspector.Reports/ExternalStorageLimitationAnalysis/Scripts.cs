namespace KInspector.Reports.ExternalStorageLimitationAnalysis
{
    public static class Scripts
    {
        public static string BaseDirectory => $"{nameof(ExternalStorageLimitationAnalysis)}/Scripts";

        public static string GetMediaFoldersWithMoreThan100Files => $"{BaseDirectory}/{nameof(GetMediaFoldersWithMoreThan100Files)}.sql";
    }
}
