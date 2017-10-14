using System.Collections.Generic;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class BuilderConfig
    {
        public string OutputFile { get; set; } = string.Empty;

        public IList<FileConfig> Files { get; set; } = new List<FileConfig>();
        public IList<PatchConfig> Patches { get; set; } = new List<PatchConfig>();
        public IList<TableConfig> Tables { get; set; } = new List<TableConfig>();
        
        public bool Exrom { get; set; }
        public bool Game { get; set; }
        public int Hardware { get; set; }
        public string Name { get; set; }
        
        public int? Capacity { get; set; }
    }
}