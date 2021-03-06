﻿using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// An autogenerated patch.
    /// </summary>
    public interface IFill
    {
        /// <summary>
        /// Data bytes in the pattern.
        /// </summary>
        byte[] Data { get; }
        
        /// <summary>
        /// Length of the patch.
        /// </summary>
        int Length { get; }
        
        /// <summary>
        /// Offset of the patch.
        /// </summary>
        int Offset { get; }
        
        /// <summary>
        /// Wrap strategy to use when populating the patch.
        /// </summary>
        WrapStrategy WrapStrategy { get; }
        
        /// <summary>
        /// Starting bank of the patch.
        /// </summary>
        int Bank { get; }
        
        /// <summary>
        /// If true, the filled area is eligible for deduplication.
        /// </summary>
        bool Dedupe { get; }
    }
}