using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Crt
{
    public class Chip : IChip
    {
        public int Type { get; set; }
        public int Bank { get; set; }
        public int Address { get; set; }
        public IList<byte> Rom { get; set; }
        public IList<byte> ExtraData { get; set; }
    }
}