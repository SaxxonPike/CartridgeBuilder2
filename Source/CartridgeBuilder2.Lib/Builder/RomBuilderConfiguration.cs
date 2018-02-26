using System.Collections.Generic;
using CartridgeBuilder2.Lib.Infrastructure;
using CartridgeBuilder2.Lib.Prg;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc/>
    [Model]
    public class RomBuilderConfiguration : IRomBuilderConfiguration
    {
        /// <inheritdoc/>
        public IList<IFile> Files { get; set; }
        /// <inheritdoc/>
        public IList<IPatch> Patches { get; set; }
        /// <inheritdoc/>
        public IList<ITable> Tables { get; set; }
        /// <inheritdoc/>
        public IList<IFill> Fills { get; set; }
        /// <inheritdoc/>
        public int? Capacity { get; set; }
    }
}