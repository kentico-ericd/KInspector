using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using KInspector.Reports.ContentTreeChildrenAnalysis.Models;

namespace KInspector.Reports.ContentTreeChildrenAnalysis
{
    public class Report : AbstractReport<Terms>
    {
        private readonly IDatabaseService databaseService;

        public override IList<Version> CompatibleVersions => VersionHelper.GetVersionList("10", "11", "12", "13");

        public override IList<string> Tags => new List<string> {
            ModuleTags.Health,
            ModuleTags.Performance
        };

        public Report(IDatabaseService databaseService, IModuleMetadataService moduleMetadataService) : base(moduleMetadataService)
        {
            this.databaseService = databaseService;
        }

        public async override Task<ModuleResults> GetResults()
        {
            var nodes = await databaseService.ExecuteSqlFromFile<CmsTreeNode>(Scripts.GetNodesWithManyChildren);
            if (nodes.Any())
            {
                var results = new ModuleResults
                {
                    Status = ResultsStatus.Error,
                    Type = ResultsType.TableList,
                    Summary = Metadata.Terms.IssuesFound?.With(new { totalIssues = nodes.Count() })
                };
                results.TableResults.Add(new TableResult
                {
                    Name = Metadata.Terms.BadNodesTable,
                    Rows = nodes
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
