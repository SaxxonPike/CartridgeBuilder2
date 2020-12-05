using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Represents a file that has been packed into a ROM.
    /// </summary>
    public interface IPackedFile
    {
        /// <summary>
        /// Name of the file.
        /// </summary>
        byte[] Name { get; }
        
        /// <summary>
        /// Load address for the file.
        /// </summary>
        int? LoadAddress { get; }
        
        /// <summary>
        /// The starting address, mapped to the system's ROM address space.
        /// </summary>
        int StartAddress { get; }
        
        /// <summary>
        /// The starting bank of the file data.
        /// </summary>
        int StartBank { get; }
        
        /// <summary>
        /// Strategy used to store the file data.
        /// </summary>
        WrapStrategy WrapStrategy { get; }
        
        /// <summary>
        /// Length of the file data, in bytes.
        /// </summary>
        int Length { get; }
        
        /// <summary>
        /// Compression method used for the file data.
        /// </summary>
        CompressionMethod CompressionMethod { get; }
    }
}