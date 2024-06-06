namespace KInspector.Reports.KenticoInstanceDetailSummary
{
    public static class Scripts
    {
        public static string BaseDirectory => $"{nameof(KenticoInstanceDetailSummary)}/Scripts";

        public static string GetSiteDetailsV12 => $"{BaseDirectory}/{nameof(GetSiteDetailsV12)}.sql";

        public static string GetSiteDetailsV13=> $"{BaseDirectory}/{nameof(GetSiteDetailsV13)}.sql";
    }
}