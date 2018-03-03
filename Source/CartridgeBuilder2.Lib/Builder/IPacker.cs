using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Maps and writes data into a ROM space.
    /// </summary>
    public interface IPacker
    {
        /// <summary>
        /// Gets the best heuristically determined place in ROM space to write the specified number of bytes.
        /// </summary>
        IAllocation Fit(IRomSpace romSpace, int size);
        
        /// <summary>
        /// Writes the specified data to the specified place in ROM. 
        /// </summary>
        IAllocation Write(IRomSpace romSpace, IEnumerable<byte> data, IAllocation allocation, OverwriteRule overwriteRule);
        
        /// <summary>
        /// Reserves the specified place in ROM so that the Fit method will not consider it for file placement.
        /// </summary>
        IAllocation Reserve(IRomSpace romSpace, IAllocation allocation);
    }
}