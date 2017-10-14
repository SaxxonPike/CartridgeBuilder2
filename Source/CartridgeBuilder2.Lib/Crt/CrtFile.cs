using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Crt
{
    public class CrtFile : ICrtFile
    {
        public ICrtInfo Info { get; set; }
        public IList<IChip> Chips { get; set; }
    }
}