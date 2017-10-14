using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Crt
{
    public class CrtInfo : ICrtInfo
    {
        public int Version { get; set; }
        public int Hardware { get; set; }
        public bool ExromPin { get; set; }
        public bool GamePin { get; set; }
        public IList<byte> ExtraData { get; set; }
        public IList<byte> ReservedData { get; set; }
        public string Name { get; set; }
    }
}