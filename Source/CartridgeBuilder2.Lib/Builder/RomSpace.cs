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
        }

        /// <inheritdoc/>
        public IList<byte> Data { get; }
        /// <inheritdoc/>
        public IList<UsageType> Usage { get; }
    }
}