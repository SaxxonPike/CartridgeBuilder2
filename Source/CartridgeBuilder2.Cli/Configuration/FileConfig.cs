using System.ComponentModel.DataAnnotations;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class FileConfig
    {
        [Required]
        public string Path { get; set; }

        public string Name { get; set; }
        public int? LoadAddress { get; set; }
        public bool Dedupe { get; set; } = true;
    }
}