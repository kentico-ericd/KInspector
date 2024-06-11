using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using KInspector.Reports.DatabaseTableSizeAnalysis.Models;

namespace KInspector.Reports.DatabaseTableSizeAnalysis
{
    public class Report : AbstractReport<Terms>
    {
        private readonly IDatabaseService databaseService;

        public Report(IDatabaseService databaseService, IModuleMetadataService moduleMetadataService) : base(moduleMetadataService)
        {
            this.databaseService = databaseService;
        }

        public override IList<Version> CompatibleVersions => VersionHelper.GetVersionList("10", "11", "12", "13");

        public override IList<string> Tags => new List<string> {
            ModuleTags.Health
        };

        public async override Task<ModuleResults> GetResults()
        {
            var top25LargestTables = await databaseService.ExecuteSqlFromFile<DatabaseTableSizeResult>(Scripts.GetTop25LargestTables);
            var results = new ModuleResults
            {
                Type = ResultsType.NoResults,
                Status = ResultsStatus.Information,
                Summary = Metadata.Terms.Summaries?.CheckResultsTableForAnyIssues
            };
            if (top25LargestTables.Any())
            {
                results.Type = ResultsType.TableList;
                results.TableResults.Add(new TableResult
                {
                    Name = Metadata.Terms.TableTitles?.Top25Results,
                    Rows = top25LargestTables
                });
            }

            return results;
        }
    }
}