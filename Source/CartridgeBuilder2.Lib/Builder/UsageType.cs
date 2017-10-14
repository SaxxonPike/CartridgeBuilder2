namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Determines how space in a ROM is used.
    /// </summary>
    public enum UsageType
    {
        /// <summary>
        /// The ROM space is not used.
        /// </summary>
        Unused,
        
        /// <summary>
        /// The ROM space is occupied by data.
        /// </summary>
        Used,
        
        /// <summary>
        /// The ROM space is not used, but is reserved for non-file use.
        /// </summary>
        Reserved
    }
}