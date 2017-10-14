using System.Collections.Generic;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc/>
    [Model]
    public class PackedFile : IPackedFile
    {
        /// <inheritdoc/>
        public IList<byte> Name { get; set; }
        /// <inheritdoc/>
        public int? LoadAddress { get; set; }
        /// <inheritdoc/>
        public int StartAddress { get; set; }
        /// <inheritdoc/>
        public int StartBank { get; set; }
        /// <inheritdoc/>
        public WrapStrategy WrapStrategy { get; set; }
        /// <inheritdoc/>
        public int Length { get; set; }
        /// <inheritdoc/>
        public CompressionMethod CompressionMethod { get; set; }
    }
}