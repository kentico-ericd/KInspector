using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using KInspector.Reports.EventLogErrorSummary.Models;

namespace KInspector.Reports.EventLogErrorSummary
{
    public class Report : AbstractReport<Terms>
    {
        private readonly IDatabaseService databaseService;

        public Report(IDatabaseService databaseService, IModuleMetadataService moduleMetadataService) : base(moduleMetadataService)
        {
            this.databaseService = databaseService;
        }

        public override IList<Version> CompatibleVersions => VersionHelper.GetVersionList("12", "13");

        public override IList<string> Tags => new List<string> {
            ModuleTags.Health,
            ModuleTags.Information
        };

        public async override Task<ModuleResults> GetResults()
        {
            var errors = await databaseService.ExecuteSqlFromFile<CmsErrorEvent>(Scripts.GetEventLogErrors);
            if (!errors.Any()) {
                return new ModuleResults
                {
                    Status = ResultsStatus.Good,
                    Summary = Metadata.Terms.Summaries?.Good,
                    Type = ResultsType.NoResults
                };
            }

            var totalErrors = errors.Sum(e => e.Count);
            var results = new ModuleResults
            {
                Status = ResultsStatus.Warning,
                Summary = Metadata.Terms.Summaries?.Information?.With(new { totalErrors }),
                Type = ResultsType.TableList
            };
            results.TableResults.Add(new TableResult
            {
                Name = Metadata.Terms.TableTitles?.EventLogTableName,
                Rows = errors
            });

            return results;
        }
    }
}
