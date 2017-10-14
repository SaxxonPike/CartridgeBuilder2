using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface ICrtFile
    {
        ICrtInfo Info { get; }
        IList<IChip> Chips { get; }
    }
}