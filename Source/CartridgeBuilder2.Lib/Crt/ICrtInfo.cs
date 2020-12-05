using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface ICrtInfo
    {
        int Version { get; }
        int Hardware { get; }
        bool ExromPin { get; }
        bool GamePin { get; }
        byte[] ExtraData { get; }
        byte[] ReservedData { get; }
        string Name { get; }
    }
}