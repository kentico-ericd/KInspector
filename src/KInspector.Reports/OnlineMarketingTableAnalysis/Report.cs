using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using KInspector.Reports.OnlineMarketingTableAnalysis.Models;

namespace KInspector.Reports.OnlineMarketingTableAnalysis
{
    public class Report : AbstractReport<Terms>
    {
        private readonly IDatabaseService databaseService;
        private const int MAX_ACTIVITIES = 1000000000;
        private const int MAX_CONTACTS = 100000000;
        private const int MAX_CONTACTGROUPS = 50;
        private const int MAX_SCORINGRULES = 50;

        public override IList<Version> CompatibleVersions => VersionHelper.GetVersionList("10", "11", "12", "13");

        public override IList<string> Tags => new List<string> {
            ModuleTags.Health,
            ModuleTags.Performance,
            ModuleTags.OnlineMarketing
        };

        public Report(IDatabaseService databaseService, IModuleMetadataService moduleMetadataService) : base(moduleMetadataService)
        {
            this.databaseService = databaseService;
        }

        public async override Task<ModuleResults> GetResults()
        {
            var totalIssues = 0;
            var results = new ModuleResults
            {
                Type = ResultsType.StringList
            };
            var totalActivities = await databaseService.ExecuteSqlFromFileScalar<int>(Scripts.GetActivityCount);
            results.StringResults.Add(Metadata.Terms.ActivityIssues?.With(new
            {
                totalActivities,
                maxActivities = MAX_ACTIVITIES
            }));
            if (totalActivities > MAX_ACTIVITIES)
            {
                totalIssues++;
            }

            var totalContacts = await databaseService.ExecuteSqlFromFileScalar<int>(Scripts.GetContactCount);
            results.StringResults.Add(Metadata.Terms.ContactIssues?.With(new
            {
                totalContacts,
                maxContacts = MAX_CONTACTS
            }));
            if (totalContacts > MAX_CONTACTS)
            {
                totalIssues++;
            }

            var totalContactGroups = await databaseService.ExecuteSqlFromFileScalar<int>(Scripts.GetContactGroupCount);
            results.StringResults.Add(Metadata.Terms.ContactGroupIssues?.With(new
            {
                totalContactGroups,
                maxContactGroups = MAX_CONTACTGROUPS
            }));
            if (totalContactGroups > MAX_CONTACTGROUPS)
            {
                totalIssues++;
            }

            var totalScoringRules = await databaseService.ExecuteSqlFromFileScalar<int>(Scripts.GetContactGroupCount);
            results.StringResults.Add(Metadata.Terms.ScoringRuleIssues?.With(new
            {
                totalScoringRules,
                maxScoringRules = MAX_SCORINGRULES
            }));
            if (totalScoringRules > MAX_SCORINGRULES)
            {
                totalIssues++;
            }

            if (totalIssues > 0)
            {
                results.Status = ResultsStatus.Warning;
                results.Summary = Metadata.Terms.IssuesFound?.With(new { totalIssues });
            }
            else
            {
                results.Status = ResultsStatus.Good;
                results.Summary = Metadata.Terms.Good;
            }

            return results;
        }
    }
}
