using System.Collections.Generic;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc />
    [Model]
    public class Patch : IPatch
    {
        /// <inheritdoc />
        public int Bank { get; set; }
        /// <inheritdoc />
        public int Offset { get; set; }
        /// <inheritdoc />
        public IList<byte> Data { get; set; }
        /// <inheritdoc />
        public WrapStrategy WrapStrategy { get; set; }
    }
}