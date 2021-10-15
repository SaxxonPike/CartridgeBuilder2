using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class FileConfig
    {
        [Required]
        public string Path { get; set; }

        public string Name { get; set; }
        [JsonConverter(typeof(HexNumberJsonConverter<int?>))]
        public int? LoadAddress { get; set; }
        public bool Dedupe { get; set; } = true;
    }
}