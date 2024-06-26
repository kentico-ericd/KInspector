using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using KInspector.Reports.KenticoInstanceDetailSummary.Models;
using System.Text;

namespace KInspector.Reports.KenticoInstanceDetailSummary
{
    public class Report : AbstractReport<Terms>
    {
        private readonly IDatabaseService databaseService;
        private readonly IConfigService configService;
        private readonly IInstanceService instanceService;

        public override IList<Version> CompatibleVersions => VersionHelper.GetVersionList("12", "13");

        public override IList<string> Tags => new List<string>
        {
            ModuleTags.Information
        };

        public Report(
            IDatabaseService databaseService,
            IModuleMetadataService moduleMetadataService,
            IConfigService configService,
            IInstanceService instanceService) : base(moduleMetadataService)
        {
            this.databaseService = databaseService;
            this.configService = configService;
            this.instanceService = instanceService;
        }

        public async override Task<ModuleResults> GetResults()
        {
            var instance = configService.GetCurrentInstance() ??
                throw new InvalidOperationException("No connected instance.");
            var instanceDetails = instanceService.GetInstanceDetails(instance) ??
                throw new InvalidOperationException("Unable to retrieve instance details.");

            var resultBuilder = new StringBuilder();
            var results = new ModuleResults
            {
                Type = ResultsType.MarkdownString,
                Status = ResultsStatus.Information,
                Summary = Metadata.Terms.Information
            };

            // Global
            var globalDetails = await databaseService.ExecuteSqlFromFileScalar<string>(Scripts.GetGlobalDetails);
            resultBuilder.AppendLine(globalDetails);

            // Loop through sites
            var siteScript = GetSiteScriptFile(instanceDetails);
            foreach (var site in instanceDetails.AllSites)
            {
                var detail = await databaseService.ExecuteSqlFromFileScalar<string>(siteScript, new { SiteId = site.Id });
                resultBuilder.AppendLine(detail);
            }
            results.StringResult = resultBuilder.ToString();

            return results;
        }

        private static string GetSiteScriptFile(InstanceDetails instanceDetails) =>
            (instanceDetails.DatabaseVersion?.Major) switch
            {
                12 => Scripts.GetSiteDetailsV12,
                13 => Scripts.GetSiteDetailsV13,
                _ => throw new InvalidOperationException("Unsupported Kentico version.")
            };
    }
}
