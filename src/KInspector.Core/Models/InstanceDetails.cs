namespace KInspector.Core.Models
{
    /// <summary>
    /// Represents additional information about an instance not stored in the application's configuration file.
    /// </summary>
    public class InstanceDetails
    {
        /// <summary>
        /// The version of the Kentico DLLs in the administration website.
        /// </summary>
        public Version? AdministrationVersion { get; set; }

        /// <summary>
        /// The version of the Kentico DLLs in the administration website.
        /// </summary>
        public Version? AdministrationDatabaseVersion { get; set; }

        /// <summary>
        /// The selected Kentico site.
        /// </summary>
        public Site? Site { get; set; }

        /// <summary>
        /// The sites contained in the instance's CMS_Site table.
        /// </summary>
        public IEnumerable<Site> AllSites { get; set; } = Enumerable.Empty<Site>();
    }
}