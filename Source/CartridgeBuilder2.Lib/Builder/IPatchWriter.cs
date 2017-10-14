using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Applies patches to a ROM.
    /// </summary>
    public interface IPatchWriter
    {
        /// <summary>
        /// Apply the specified patches to the ROM.
        /// </summary>
        void Write(IRomSpace romSpace, IEnumerable<IPatch> patches);
    }
}