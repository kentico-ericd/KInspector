using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;
using KInspector.Reports.ExternalStorageLimitationAnalysis.Models;

namespace KInspector.Reports.ExternalStorageLimitationAnalysis
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
            ModuleTags.Performance,
            ModuleTags.Azure,
            ModuleTags.AmazonS3
        };

        public async override Task<ModuleResults> GetResults()
        {
            var foldersOverLimit = await databaseService.ExecuteSqlFromFile<CmsMediaFolderResult>(Scripts.GetMediaFoldersWithMoreThan100Files);
            if (foldersOverLimit.Any())
            {
                var result = new ModuleResults
                {
                    Status = ResultsStatus.Error,
                    Type = ResultsType.TableList,
                    Summary = Metadata.Terms.Summaries?.Error?.With(new { totalFolders = foldersOverLimit.Count() })
                };
                result.TableResults.Add(new TableResult
                {
                    Name = Metadata.Terms.TableTitles?.TablesOverLimit,
                    Rows = foldersOverLimit
                });

                return result;
            }

            return new ModuleResults
            {
                Status = ResultsStatus.Good,
                Type = ResultsType.NoResults,
                Summary = Metadata.Terms.Summaries?.Good
            };
        }
    }
}
