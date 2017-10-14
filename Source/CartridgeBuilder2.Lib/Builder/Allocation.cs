using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc/>
    [Model]
    public class Allocation : IAllocation
    {
        /// <inheritdoc/>
        public CompressionMethod CompressionMethod { get; set; }
        /// <inheritdoc/>
        public int Offset { get; set; }
        /// <inheritdoc/>
        public int? Length { get; set; }
        /// <inheritdoc/>
        public WrapStrategy WrapStrategy { get; set; }
    }
}