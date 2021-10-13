using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CartridgeBuilder2.Lib.Builder;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class FillConfig
    {
        [Required]
        public int Bank { get; set; }

        [Required]
        public int Offset { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WrapStrategy WrapStrategy { get; set; }

        [Required]
        public byte[] Bytes { get; set; }
        
        [Required]
        public int Length { get; set; } 
        
        public bool Dedupe { get; set; }
    }
}