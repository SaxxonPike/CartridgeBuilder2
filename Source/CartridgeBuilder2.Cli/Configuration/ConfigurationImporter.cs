using System.IO;
using CartridgeBuilder2.Lib.Infrastructure;
using Newtonsoft.Json;

namespace CartridgeBuilder2.Cli.Configuration
{
    [Service]
    public class ConfigurationImporter : IConfigurationImporter
    {
        public BuilderConfig Import(Stream stream)
        {
            var textReader = new StreamReader(stream);
            var text = textReader.ReadToEnd();
            return JsonConvert.DeserializeObject<BuilderConfig>(text);
        }
    }
}