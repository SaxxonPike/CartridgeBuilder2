namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Determines the organization of data for a file.
    /// </summary>
    public enum WrapStrategy
    {
        /// <summary>
        /// The data uses both the lower (8xxx) and upper (Axxx/Exxx) ROMs.
        /// </summary>
        Both,
        
        /// <summary>
        /// The data uses only the lower (8xxx) ROM.
        /// </summary>
        Low,
        
        /// <summary>
        /// The data uses only the upper (Axxx/Exxx) ROM.
        /// </summary>
        High
    }
}