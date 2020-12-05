using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Builds tables based on packed file data.
    /// </summary>
    public interface ITableBuilder
    {
        /// <summary>
        /// Build a table using the specified table configuration and file table.
        /// </summary>
        byte[] Build(ITable table, IEnumerable<IPackedFile> files);
    }
}