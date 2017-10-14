using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Applies table data to a ROM.
    /// </summary>
    public interface ITableWriter
    {
        /// <summary>
        /// Apply the data of all tables to the specified ROM.
        /// </summary>
        void Write(IRomSpace romSpace, IEnumerable<ITable> tables, IEnumerable<IPackedFile> packedFiles);
        
        /// <summary>
        /// Reserve space in the ROM for all tables.
        /// </summary>
        void Reserve(IRomSpace romSpace, IEnumerable<ITable> tables);
    }
}