using CartridgeBuilder2.Lib.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class TableConfig
    {
        [JsonRequired]
        [JsonConverter(typeof(StringEnumConverter))]
        public TableType Type { get; set; }

        [JsonRequired]
        public int Bank { get; set; }

        [JsonRequired]
        public int Offset { get; set; }

        public WrapStrategy Usage { get; set; }

        public int Index { get; set; }
        
        public int? Length { get; set; }
    }
}