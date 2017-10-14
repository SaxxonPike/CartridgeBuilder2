using CartridgeBuilder2.Lib.Builder;
using Newtonsoft.Json;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class PatchConfig
    {
        [JsonRequired]
        public int Bank { get; set; }

        [JsonRequired]
        public int Offset { get; set; }

        public WrapStrategy Usage { get; set; }

        [JsonRequired]
        public string Path { get; set; }
    }
}