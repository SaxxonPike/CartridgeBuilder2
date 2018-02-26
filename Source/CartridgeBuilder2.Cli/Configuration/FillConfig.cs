using CartridgeBuilder2.Lib.Builder;
using Newtonsoft.Json;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class FillConfig
    {
        [JsonRequired]
        public int Bank { get; set; }

        [JsonRequired]
        public int Offset { get; set; }

        public WrapStrategy WrapStrategy { get; set; }

        [JsonRequired]
        public byte[] Bytes { get; set; }
        
        [JsonRequired]
        public int Length { get; set; } 
        
        public bool Dedupe { get; set; }
    }
}