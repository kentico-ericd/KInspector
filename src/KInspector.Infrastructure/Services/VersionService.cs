using System.Diagnostics;

using KInspector.Core.Models;
using KInspector.Core.Services.Interfaces;

namespace KInspector.Infrastructure.Services
{
    public class VersionService : IVersionService
    {
        private readonly IDatabaseService databaseService;

        private static readonly string getCmsSettingsPath = @"Scripts/GetCmsSettings.sql";

        private const string _dllToCheck = "CMS.DataEngine.dll";
        private const string _relativeBinPath = "bin";
        private const string _relativeHotfixFileFolderPath = "App_Data\\Install";
        private const string _hotfixFile = "Hotfix.txt";

        public VersionService(IDatabaseService databaseService)
        {
            this.databaseService = databaseService;
        }

        public Version? GetKenticoLiveSiteVersion(Instance instance)
        {
            if (!Directory.Exists(instance.LiveSitePath))
            {
                return null;
            }

            var versionInfo = GetKenticoDllVersion(instance.LiveSitePath);
            if (versionInfo is null)
            {
                return null;
            }

            return new Version($"{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{versionInfo.FileBuildPart}");
        }

        public Version? GetKenticoAdministrationVersion(Instance instance)
        {
            if (!Directory.Exists(instance.AdministrationPath))
            {
                return null;
            }

            var versionInfo = GetKenticoDllVersion(instance.AdministrationPath);
            if (versionInfo is null)
            {
                return null;
            }

            var hotfix = versionInfo.FileBuildPart.ToString();
            var hotfixDirectory = Path.Combine(instance.AdministrationPath, _relativeHotfixFileFolderPath);
            if (Directory.Exists(hotfixDirectory))
            {
                var hotfixFile = Path.Combine(hotfixDirectory, _hotfixFile);

                if (File.Exists(hotfixFile))
                {
                    hotfix = File.ReadAllText(hotfixFile);
                }
            }

            var version = $"{versionInfo.FileMajorPart}.{versionInfo.FileMinorPart}.{hotfix}";

            return new Version(version);
        }

        public Version? GetKenticoDatabaseVersion(DatabaseSettings databaseSettings)
        {
            databaseService.Configure(databaseSettings);
            var settingsKeys = databaseService.ExecuteSqlFromFile<string>(getCmsSettingsPath).ConfigureAwait(false).GetAwaiter().GetResult();
            var settingsList = settingsKeys.ToList();
            var version = settingsList[0];
            if (version is null)
            {
                return null;
            }

            // Xperience by Kentico stores full version in single key
            if (settingsList.Count == 1)
            {
                return new Version(version);
            }

            var hotfix = settingsList[1];
            if (hotfix is null)
            {
                return null;
            }

            return new Version($"{version}.{hotfix}");
        }

        private static FileVersionInfo? GetKenticoDllVersion(string rootPath)
        {
            string dllFileToCheck;
            var binDirectory = Path.Combine(rootPath, _relativeBinPath);
            // For published .NET Core projects, the DLLs are in the root directory. For administration and MVC projects, the DLLs are in
            // the /bin folder.
            if (Directory.Exists(binDirectory))
            {
                dllFileToCheck = Path.Combine(binDirectory, _dllToCheck);
            }
            else
            {
                dllFileToCheck = Path.Combine(rootPath, _dllToCheck);
            }


            if (!File.Exists(dllFileToCheck))
            {
                return null;
            }

            return FileVersionInfo.GetVersionInfo(dllFileToCheck);
        }
    }
}