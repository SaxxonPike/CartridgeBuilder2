namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Generates useful addresses from raw offsets within a workspace.
    /// </summary>
    public interface IAddressGenerator
    {
        /// <summary>
        /// Get the 16-bit address from an offset.
        /// </summary>
        int GetAddress(int offset);
        
        /// <summary>
        /// Get the 8-bit bank number from an offset. 
        /// </summary>
        int GetBank(int offset);
    }
}