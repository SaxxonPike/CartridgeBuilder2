using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    public class Fill : IFill
    {
        public byte[] Data { get; set; }
        public int Length { get; set; }
        public int Offset { get; set; }
        public WrapStrategy WrapStrategy { get; set; }
        public int Bank { get; set; }
        public bool Dedupe { get; set; }
    }
}