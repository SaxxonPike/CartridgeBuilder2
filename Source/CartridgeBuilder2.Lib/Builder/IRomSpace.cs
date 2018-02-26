using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Contains data and allocation for a ROM image.
    /// </summary>
    public interface IRomSpace
    {
        /// <summary>
        /// Raw ROM data.
        /// </summary>
        IList<byte> Data { get; }
        
        /// <summary>
        /// Describes how each byte of ROM is allocated.
        /// </summary>
        IList<UsageType> Usage { get; }

        /// <summary>
        /// Indicates the maximum used space.
        /// </summary>
        int DataLength { get; set; }
        
        /// <summary>
        /// Indicates lowest offset of the next available byte.
        /// </summary>
        int LowestAvailable { get; set; }
    }
}