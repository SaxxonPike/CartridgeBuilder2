using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface IChip
    {
        int Type { get; }
        int Bank { get; }
        int Address { get; }
        IList<byte> Rom { get; }
        IList<byte> ExtraData { get; }
    }
}