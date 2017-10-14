using System.Collections.Generic;
using CartridgeBuilder2.Lib.Crt;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Exports a flat ROM image to CRT chip blocks.
    /// </summary>
    public interface IRomChipExporter
    {
        /// <summary>
        /// Export the specified ROM to CRT chip blocks.
        /// </summary>
        IEnumerable<IChip> Export(IRomSpace romSpace);
    }
}