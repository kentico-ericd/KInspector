using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using KInspector.Reports.InactiveContactSettingsSummary.Models;

namespace KInspector.Reports.InactiveContactSettingsSummary
{
    public class Report : AbstractReport<Terms>
    {
        private readonly IDatabaseService databaseService;

        public override IList<Version> CompatibleVersions => VersionHelper.GetVersionList("12", "13");

        public override IList<string> Tags => new List<string> {
            ModuleTags.Health,
            ModuleTags.Performance,
            ModuleTags.OnlineMarketing,
            ModuleTags.Configuration
        };

        public Report(IDatabaseService databaseService, IModuleMetadataService moduleMetadataService) : base(moduleMetadataService)
        {
            this.databaseService = databaseService;
        }

        public async override Task<ModuleResults> GetResults()
        {
            var data = await databaseService.ExecuteSqlFromFile<CmsSettingsKey>(Scripts.GetInactiveContactDeletionSettings);
            var results = new ModuleResults
            {
                Status = ResultsStatus.Information,
                Type = ResultsType.TableList,
                Summary = Metadata.Terms.Information
            };
            results.TableResults.Add(new TableResult
            {
                Name = Metadata.Terms.SettingsTableName,
                Rows = data
            });

            return results;
        }
    }
}
