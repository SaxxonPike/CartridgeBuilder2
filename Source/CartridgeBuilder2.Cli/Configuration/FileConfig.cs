using Newtonsoft.Json;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class FileConfig
    {
        [JsonRequired]
        public string Path { get; set; }

        public string Name { get; set; }
        public int? LoadAddress { get; set; }
        public bool Dedupe { get; set; } = true;
    }
}