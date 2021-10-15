using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using CartridgeBuilder2.Lib.Builder;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class PatchConfig
    {
        [Required]
        [JsonConverter(typeof(HexNumberJsonConverter<int?>))]
        public int? Bank { get; set; }

        [Required]
        [JsonConverter(typeof(HexNumberJsonConverter<int>))]
        public int Offset { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public WrapStrategy WrapStrategy { get; set; }

        [Required]
        public string Path { get; set; }

        public bool Dedupe { get; set; }
    }
}