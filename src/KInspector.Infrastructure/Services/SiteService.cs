using Dapper;

using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;

namespace KInspector.Infrastructure.Services
{
    public class SiteService : ISiteService
    {
        public Site? GetSite(Instance instance)
        {
            var sites = GetSites(instance.DatabaseSettings);

            return sites.FirstOrDefault(s => s.Id == instance.SiteId);
        }

        public IList<Site> GetSites(DatabaseSettings? databaseSettings)
        {
            ArgumentNullException.ThrowIfNull(databaseSettings);

            try
            {
                var query = @"
                    SELECT
                        SiteId as Id,
                        SiteName as Name,
                        SiteGUID as Guid,
                        SiteDomainName as DomainName,
                        SitePresentationURL as PresentationUrl,
                        SiteStatus as Status
                    FROM CMS_Site";

                var connection = DatabaseHelper.GetSqlConnection(databaseSettings);
                var sites = connection.Query<Site>(query).ToList();

                return sites;
            }
            catch
            {
                return Array.Empty<Site>();
            }
        }
    }
}
