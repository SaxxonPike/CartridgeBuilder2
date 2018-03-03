using System.Collections.Generic;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc/>
    [Model]
    public class RomSpace : IRomSpace
    {
        public RomSpace(int capacity)
        {
            Data = new byte[capacity];
            Usage = new UsageType[capacity];
            DataContentLength = 0;
            LowestAvailable = 0;
        }

        /// <inheritdoc/>
        public IList<byte> Data { get; }
        /// <inheritdoc/>
        public IList<UsageType> Usage { get; }
        /// <inheritdoc/>
        public int DataContentLength { get; set; }
        /// <inheritdoc/>
        public int LowestAvailable { get; set; }
    }
}