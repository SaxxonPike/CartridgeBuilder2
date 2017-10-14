using CartridgeBuilder2.Lib.Builder;

namespace CartridgeBuilder2.Cli.Configuration
{
    public interface IConfigurationMapper
    {
        IRomBuilderConfiguration MapRomBuilderConfiguration(BuilderConfig config);
    }
}