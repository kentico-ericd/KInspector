using Dapper;

using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using System.Data.SqlTypes;

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

                string sqlFile = dbVersion.Major switch
                {
                    13 => "Scripts/GetCmsSitesV13.sql",
                    27 or 28 or 29 => "Scripts/GetCmsSitesXbK.sql",
                    _ => throw new InvalidOperationException("Unsupported Kentico version."),
                };
                var connection = DatabaseHelper.GetSqlConnection(databaseSettings);
                var query = FileHelper.GetSqlQueryText(sqlFile);
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
