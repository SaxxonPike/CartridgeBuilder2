using System.IO;
using System.Text.Json;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Cli.Configuration
{
    [Service(typeof(IConfigurationImporter))]
    public class ConfigurationImporter : IConfigurationImporter
    {
        public BuilderConfig Import(Stream stream)
        {
            var textReader = new StreamReader(stream);
            var text = textReader.ReadToEnd();
            return JsonSerializer.Deserialize<BuilderConfig>(text);
        }
    }
}