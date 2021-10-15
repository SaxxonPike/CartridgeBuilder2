using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CartridgeBuilder2.Lib.Builder;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class TableConfig
    {
        [Required]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TableType Type { get; set; }

        [Required]
        [JsonConverter(typeof(HexNumberJsonConverter<int>))]
        public int Bank { get; set; }

        [Required]
        [JsonConverter(typeof(HexNumberJsonConverter<int>))]
        public int Offset { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WrapStrategy WrapStrategy { get; set; }

        [JsonConverter(typeof(HexNumberJsonConverter<int>))]
        public int Index { get; set; }
        
        [JsonConverter(typeof(HexNumberJsonConverter<int?>))]
        public int? Length { get; set; }
        
        [JsonConverter(typeof(HexNumberJsonConverter<int?>))]
        public int? Mask { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CaseType? Case { get; set; }
    }
}