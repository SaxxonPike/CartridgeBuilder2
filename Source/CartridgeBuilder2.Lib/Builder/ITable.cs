namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Contains information about an auto-generated table.
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Type of table to generate.
        /// </summary>
        TableType Type { get; }
        
        /// <summary>
        /// ROM bank in which this table should be located.
        /// </summary>
        int Bank { get; }
        
        /// <summary>
        /// Offset within the bank that this table should be located.
        /// </summary>
        int Offset { get; }
        
        /// <summary>
        /// Maximum length of the table data.
        /// </summary>
        int Length { get; }
        
        /// <summary>
        /// For tables that refer to information that is larger than one byte (such as file name), this is the
        /// index within the referred data to pull from.
        /// </summary>
        int Index { get; }
        
        /// <summary>
        /// ROM bank wrapping strategy to use when writing the table.
        /// </summary>
        WrapStrategy WrapStrategy { get; }

        /// <summary>
        /// Bitmask for use on the output values.
        /// </summary>
        int? Mask { get; }
    }
}