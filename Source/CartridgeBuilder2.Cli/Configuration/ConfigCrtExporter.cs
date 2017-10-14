using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Crt;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Cli.Configuration
{
    [Service]
    public class ConfigCrtExporter : IConfigCrtExporter
    {
        public ICrtFile Export(BuilderConfig config, IEnumerable<IChip> chips)
        {
            return new CrtFile
            {
                Chips = chips.ToList(),
                Info = new CrtInfo
                {
                    ExromPin = config.Exrom,
                    ExtraData = new List<byte>(),
                    GamePin = config.Game,
                    Hardware = config.Hardware,
                    Name = config.Name,
                    ReservedData = new List<byte>(),
                    Version = 0x0100
                }
            };
        }
    }
}