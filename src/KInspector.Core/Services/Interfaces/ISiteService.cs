using KInspector.Core.Models;

namespace KInspector.Core.Services.Interfaces
{
    /// <summary>
    /// Contains methods for retrieving sites from a Kentico database.
    /// </summary>
    public interface ISiteService : IService
    {
        /// <summary>
        /// Gets the Kentico site selected in the <paramref name="instance"/>.
        /// </summary>
        public Site? GetSite(Instance instance);

        /// <summary>
        /// Gets all Kentico sites in the CMS_Site table.
        /// </summary>
        public IList<Site> GetSites(DatabaseSettings? databaseSettings);
    }
}
