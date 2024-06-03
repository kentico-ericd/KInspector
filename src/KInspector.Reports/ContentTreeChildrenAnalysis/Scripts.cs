namespace KInspector.Reports.ContentTreeChildrenAnalysis
{
    public static class Scripts
    {
        public static string BaseDirectory => $"{nameof(ContentTreeChildrenAnalysis)}/Scripts";

        public static string GetNodesWithManyChildren => $"{BaseDirectory}/{nameof(GetNodesWithManyChildren)}.sql";
    }
}