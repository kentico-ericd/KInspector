using Dapper;

using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;

namespace KInspector.Infrastructure.Services
{
    public class SiteService : ISiteService
    {
        private readonly IVersionService _versionService;

        public SiteService(IVersionService versionService)
        {
            _versionService = versionService;
        }

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
                var dbVersion = _versionService.GetKenticoDatabaseVersion(databaseSettings);
                if (dbVersion == null)
                {
                    return new List<Site>();
                }

                string query;
                switch (dbVersion.Major)
                {
                    case 13:
                        query = @"
                        SELECT
                            SiteId as Id,
                            SiteName as Name,
                            SiteGUID as Guid,
                            SiteDomainName as DomainName,
                            SitePresentationURL as PresentationUrl,
                            SiteStatus as Status
                        FROM CMS_Site";
                        break;
                    case 27:
                    case 28:
                    case 29:
                        query = @"
                        SELECT 
	                        ChannelID as Id,
	                        ChannelName as Name,
	                        ChannelGUID as Guid,
	                        WebsiteChannelDomain as DomainName,
                            'RUNNING' as Status
                        FROM CMS_Channel C
                        JOIN CMS_WebsiteChannel W ON C.ChannelID = W.WebsiteChannelChannelID";
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported Kentico version.");
                }

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
