using System.IO;

namespace CartridgeBuilder2.Cli.Configuration
{
    public interface IConfigurationImporter
    {
        BuilderConfig Import(Stream stream);
    }
}