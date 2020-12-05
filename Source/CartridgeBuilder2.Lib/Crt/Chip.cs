using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Crt
{
    public class Chip : IChip
    {
        public int Type { get; set; }
        public int Bank { get; set; }
        public int Address { get; set; }
        public byte[] Rom { get; set; }
        public byte[] ExtraData { get; set; }
    }
}