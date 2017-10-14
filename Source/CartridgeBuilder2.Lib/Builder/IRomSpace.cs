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
    }
}