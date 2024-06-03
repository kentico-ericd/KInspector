using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using KInspector.Reports.PageNotFoundSummary.Models;

namespace KInspector.Reports.PageNotFoundSummary
{
    public class Report : AbstractReport<Terms>
    {
        private readonly IDatabaseService databaseService;

        public override IList<Version> CompatibleVersions => VersionHelper.GetVersionList("10", "11", "12", "13");

        public override IList<string> Tags => new List<string> {
            ModuleTags.Health,
            ModuleTags.Information
        };

        public Report(IDatabaseService databaseService, IModuleMetadataService moduleMetadataService) : base(moduleMetadataService)
        {
            this.databaseService = databaseService;
        }

        public async override Task<ModuleResults> GetResults()
        {
            var notFoundEvents = await databaseService.ExecuteSqlFromFile<CmsNotFoundEvent>(Scripts.GetPageNotFoundEventLogEntries);
            if (notFoundEvents.Any())
            {
                var results = new ModuleResults
                {
                    Status = ResultsStatus.Warning,
                    Type = ResultsType.TableList,
                    Summary = Metadata.Terms.IssuesFound?.With(new { totalIssuesFound = notFoundEvents.Count() })
                };
                results.TableResults.Add(new TableResult
                {
                    Name = Metadata.Terms.NotFoundEventsTable,
                    Rows = notFoundEvents
                });

                return results;
            }

            return new ModuleResults
            {
                Status = ResultsStatus.Good,
                Type = ResultsType.NoResults,
                Summary = Metadata.Terms.Good
            };
        }
    }
}
