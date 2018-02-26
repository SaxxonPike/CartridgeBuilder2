using System.Collections.Generic;
using CartridgeBuilder2.Lib.Builder;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class BuilderConfig
    {
        public string OutputFile { get; set; } = string.Empty;

        public IList<FileConfig> Files { get; set; } = new List<FileConfig>();
        public IList<PatchConfig> Patches { get; set; } = new List<PatchConfig>();
        public IList<TableConfig> Tables { get; set; } = new List<TableConfig>();
        public IList<FillConfig> Fills { get; set; } = new List<FillConfig>();

        public bool Exrom { get; set; } = true;
        public bool Game { get; set; } = false;
        public int Hardware { get; set; } = RomBuilderDefaults.HardwareId;
        public string Name { get; set; }

        public int? Capacity { get; set; } = RomBuilderDefaults.Capacity;
    }
}