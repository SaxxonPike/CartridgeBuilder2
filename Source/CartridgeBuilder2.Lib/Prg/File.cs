using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Prg
{
    public class File : IFile
    {
        public int? LoadAddress { get; set; }
        public byte[] Data { get; set; }
        public byte[] Name { get; set; }
        public bool Dedupe { get; set; }
        
        // not part of the config schema
        public int Index { get; set; }
    }
}