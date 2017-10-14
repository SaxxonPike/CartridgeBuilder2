using System.Collections.Generic;
using CartridgeBuilder2.Lib.Prg;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Contains source data for use in building ROMs.
    /// </summary>
    public interface IRomBuilderConfiguration
    {
        /// <summary>
        /// Contains files to compile.
        /// </summary>
        IList<IFile> Files { get; }
        
        /// <summary>
        /// Contains static data (patches) to compile.
        /// </summary>
        IList<IPatch> Patches { get; }
        
        /// <summary>
        /// Contains information about automatically generated static data to compile.
        /// </summary>
        IList<ITable> Tables { get; }
        
        /// <summary>
        /// Size of the output ROM.
        /// </summary>
        int? Capacity { get; }
    }
}