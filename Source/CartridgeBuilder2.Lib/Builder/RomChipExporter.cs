using System;
using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Crt;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc />
    [Service]
    public class RomChipExporter : IRomChipExporter
    {
        private readonly IAddressGenerator _addressGenerator;

        public RomChipExporter(IAddressGenerator addressGenerator)
        {
            _addressGenerator = addressGenerator;
        }
        
        /// <inheritdoc />
        public IEnumerable<IChip> Export(IRomSpace romSpace)
        {
            var romPages = romSpace.Data.Paginate(0x2000).AsArray();
            var usePages = romSpace.Usage.Paginate(0x2000).AsArray();

            for (var i = 0; i < romPages.Length; i++)
            {
                if (usePages[i].All(u => u == UsageType.Unused)) 
                    continue;
                
                var offset = i * 0x2000;
                yield return new Chip
                {
                    Address = _addressGenerator.GetAddress(offset),
                    Bank = _addressGenerator.GetBank(offset),
                    ExtraData = Array.Empty<byte>(),
                    Rom = romPages[i],
                    Type = 0
                };
            }
        }
    }
}