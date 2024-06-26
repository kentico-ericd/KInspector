using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;

namespace KInspector.Infrastructure.Services
{
    public class InstanceService : IInstanceService
    {
        private readonly IConfigService _configService;
        private readonly IVersionService _versionService;
        private readonly ISiteService _siteService;

        public InstanceService(
            IConfigService configService,
            IVersionService versionService,
            ISiteService siteService)
        {
            _configService = configService;
            _versionService = versionService;
            _siteService = siteService;
        }

        public InstanceDetails GetInstanceDetails(Guid instanceGuid)
        {
            var instance = _configService.GetInstance(instanceGuid) ?? throw new InvalidOperationException($"No instance with GUID '{instanceGuid}.'");

            return GetInstanceDetails(instance);
        }

        public InstanceDetails GetInstanceDetails(Instance? instance)
        {
            ArgumentNullException.ThrowIfNull(instance);

            var dbVersion = _versionService.GetKenticoDatabaseVersion(instance.DatabaseSettings);
            var instanceDetails = new InstanceDetails
            {
                LiveSiteVersion = _versionService.GetKenticoLiveSiteVersion(instance),
                AdministrationVersion = _versionService.GetKenticoAdministrationVersion(instance),
                DatabaseVersion = dbVersion,
                AllSites = _siteService.GetSites(instance.DatabaseSettings),
                Site = _siteService.GetSite(instance)
            };

            return instanceDetails;
        }
    }
}