﻿using KInspector.Actions.DisableWebFarmServers.Models;
using KInspector.Core;
using KInspector.Core.Constants;
using KInspector.Core.Helpers;
using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;

namespace KInspector.Actions.DisableWebFarmServers
{
    public class Action : AbstractAction<Terms,Options>
    {
        private readonly IDatabaseService databaseService;

        public override IList<Version> CompatibleVersions => VersionHelper.GetVersionList("12", "13");

        public override IList<string> Tags => new List<string> {
            ModuleTags.Configuration,
            ModuleTags.WebFarms
        };

        public Action(IDatabaseService databaseService, IModuleMetadataService moduleMetadataService) : base(moduleMetadataService)
        {
            this.databaseService = databaseService;
        }

        public async override Task<ModuleResults> Execute(Options? options)
        {
            var isValid = await ServerIsValid(options?.ServerId);
            if (!isValid)
            {
                return await GetInvalidOptionsResult();
            }

            await databaseService.ExecuteNonQuery(Scripts.DisableServer, new { ServerID = options?.ServerId });
            var result = await ExecuteListing();
            result.Status = ResultsStatus.Good;
            result.Summary = Metadata.Terms.Summaries?.ServerDisabled?.With(new
            {
                serverId = options?.ServerId
            });

            return result;
        }

        public override Task<ModuleResults> ExecutePartial(Options? options)
        {
            // All options are required for this action
            throw new NotImplementedException();
        }

        public async override Task<ModuleResults> ExecuteListing()
        {
            var servers = await databaseService.ExecuteSqlFromFile<WebFarmServer>(Scripts.GetWebFarmServerSummary);
            var result = new ModuleResults
            {
                Type = ResultsType.TableList,
                Status = ResultsStatus.Information,
                Summary = Metadata.Terms.Summaries?.ListSummary
            };
            result.TableResults.Add(new TableResult
            {
                Name = Metadata.Terms.TableTitles?.WebFarmServers,
                Rows = servers
            });

            return result;
        }

        public async override Task<ModuleResults> GetInvalidOptionsResult()
        {
            var result = await ExecuteListing();
            result.Status = ResultsStatus.Error;
            result.Summary = Metadata.Terms.Summaries?.InvalidOptions;

            return result;
        }

        private async Task<bool> ServerIsValid(int? serverId)
        {
            var servers = await databaseService.ExecuteSqlFromFile<WebFarmServer>(Scripts.GetWebFarmServerSummary);

            return serverId > 0 && servers.Any(s => s.ID == serverId);
        }
    }
}
