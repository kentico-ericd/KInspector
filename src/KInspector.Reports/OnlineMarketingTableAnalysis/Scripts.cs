namespace KInspector.Reports.OnlineMarketingTableAnalysis
{
    public static class Scripts
    {
        public static string BaseDirectory => $"{nameof(OnlineMarketingTableAnalysis)}/Scripts";

        public static string GetActivityCount => $"{BaseDirectory}/{nameof(GetActivityCount)}.sql";

        public static string GetContactCount => $"{BaseDirectory}/{nameof(GetContactCount)}.sql";

        public static string GetContactGroupCount => $"{BaseDirectory}/{nameof(GetContactGroupCount)}.sql";

        public static string GetScoringRuleCount => $"{BaseDirectory}/{nameof(GetScoringRuleCount)}.sql";
    }
}