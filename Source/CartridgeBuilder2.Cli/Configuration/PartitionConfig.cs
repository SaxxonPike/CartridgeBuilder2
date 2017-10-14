using CartridgeBuilder2.Lib.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class PartitionConfig
    {
        [JsonRequired]
        public int Bank { get; set; }

        [JsonRequired]
        public int BankCount { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public WrapStrategy Usage { get; set; } = WrapStrategy.Both;

        public bool Reserve { get; set; }
    }
}