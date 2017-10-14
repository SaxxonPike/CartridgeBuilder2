using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// A patch of data.
    /// </summary>
    public interface IPatch
    {
        /// <summary>
        /// Starting bank of the patch data.
        /// </summary>
        int Bank { get; }
        
        /// <summary>
        /// Offset within the partition to store the patch.
        /// </summary>
        int Offset { get; }
        
        /// <summary>
        /// Patch binary data.
        /// </summary>
        IList<byte> Data { get; }
        
        /// <summary>
        /// Bank wrapping strategy.
        /// </summary>
        WrapStrategy WrapStrategy { get; }
    }
}