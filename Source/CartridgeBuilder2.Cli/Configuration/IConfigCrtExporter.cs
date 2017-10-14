using System.Collections.Generic;
using CartridgeBuilder2.Lib.Crt;

namespace CartridgeBuilder2.Cli.Configuration
{
    public interface IConfigCrtExporter
    {
        ICrtFile Export(BuilderConfig config, IEnumerable<IChip> chips);
    }
}