using System.Collections.Generic;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc/>
    [Model]
    public class RomBuilderResult : IRomBuilderResult
    {
        /// <inheritdoc/>
        public IList<IPackedFile> Files { get; set; }
        /// <inheritdoc/>
        public IRomSpace Rom { get; set; }
    }
}