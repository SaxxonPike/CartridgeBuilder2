using System;
using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Crt;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Cli.Configuration
{
    [Service(typeof(IConfigCrtExporter))]
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
                    ExtraData = Array.Empty<byte>(),
                    GamePin = config.Game,
                    Hardware = config.Hardware,
                    Name = config.Name,
                    ReservedData = Array.Empty<byte>(),
                    Version = 0x0100
                }
            };
        }
    }
}