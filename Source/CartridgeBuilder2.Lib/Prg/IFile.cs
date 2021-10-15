using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Prg
{
    public interface IFile
    {
        int? LoadAddress { get; }
        byte[] Data { get; }
        byte[] Name { get; }
        bool Dedupe { get; }
        
        // not part of the config schema
        int Index { get; set; }
    }
}