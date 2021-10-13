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
        public int Bank { get; set; }

        [Required]
        public int Offset { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WrapStrategy WrapStrategy { get; set; }

        public int Index { get; set; }
        
        public int? Length { get; set; }
        
        public int? Mask { get; set; }
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CaseType? Case { get; set; }
    }
}