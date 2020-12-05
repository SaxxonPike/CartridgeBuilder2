using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface IChip
    {
        int Type { get; }
        int Bank { get; }
        int Address { get; }
        byte[] Rom { get; }
        byte[] ExtraData { get; }
    }
}