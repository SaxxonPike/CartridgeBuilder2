using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Prg
{
    public class File : IFile
    {
        public int? LoadAddress { get; set; }
        public IList<byte> Data { get; set; }
        public byte[] Name { get; set; }
    }
}