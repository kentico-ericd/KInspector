using KInspector.Core.Constants;
using KInspector.Core.Services.Interfaces;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace KInspector.Core.Helpers
{
    public class CmsFileService : ICmsFileService
    {
        public string? GetCMSConnectionString(string? instanceRoot)
        {
            var appSettings = GetAppSettings(instanceRoot, DefaultKenticoPaths.AppSettingsFile);
            if (appSettings is not null)
            {
                return appSettings.GetValue("ConnectionStrings")?.Value<string>("CMSConnectionString");
            }

            var webConfig = GetXmlDocument(instanceRoot, DefaultKenticoPaths.WebConfigFile);
            if (webConfig is not null)
            {
                return webConfig
                    .SelectSingleNode("/configuration/connectionStrings/add[@name='CMSConnectionString']")?
                    .Attributes?["connectionString"]?
                    .Value;
            }

            return null;
        }

        public Dictionary<string, string> GetResourceStringsFromResx(string? instanceRoot, string relativeResxFilePath = DefaultKenticoPaths.PrimaryResxFile)
        {
            var resourceXml = GetXmlDocument(instanceRoot, relativeResxFilePath);
            var resourceStringNodes = resourceXml?.SelectNodes("/root/data");
            var results = new Dictionary<string, string>();
            if (resourceStringNodes is null)
            {
                return results;
            }

            foreach (XmlNode resourceStringNode in resourceStringNodes)
            {
                var key = resourceStringNode.Attributes?["name"]?.InnerText.ToLowerInvariant();
                var value = resourceStringNode.SelectSingleNode("./value")?.InnerText;
                if (key is null || value is null)
                {
                    continue;
                }

                results.Add(key, value);
            }

            return results;
        }

        public XmlDocument? GetXmlDocument(string? instanceRoot, string relativeFilePath)
        {
            var xmlDocument = new XmlDocument();
            try
            {
                xmlDocument.Load(instanceRoot + relativeFilePath);
                return xmlDocument;
            }
            catch
            {
                return null;
            }
        }

        private static JObject? GetAppSettings(string? instanceRoot, string relativeFilePath)
        {
            try
            {
                var contents = File.ReadAllText(instanceRoot + relativeFilePath);

                return JsonConvert.DeserializeObject<JObject>(contents);
            }
            catch
            {
                return null;
            }
        }
    }
}