using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Prg
{
    public interface IFile
    {
        int? LoadAddress { get; }
        IList<byte> Data { get; }
        byte[] Name { get; }
    }
}