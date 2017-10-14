namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Determines the compression algorithm used for a file.
    /// </summary>
    public enum CompressionMethod
    {
        /// <summary>
        /// No compression algorithm. Raw data.
        /// </summary>
        None,
        
        /// <summary>
        /// The data is compressed using Exomizer.
        /// </summary>
        Exomizer
    }
}