using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Contains the output of the ROM build process.
    /// </summary>
    public interface IRomBuilderResult
    {
        /// <summary>
        /// Contains the entries of the file system.
        /// </summary>
        IList<IPackedFile> Files { get; }
        
        /// <summary>
        /// Contains the final compiled output ROM.
        /// </summary>
        IRomSpace Rom { get; }
    }
}