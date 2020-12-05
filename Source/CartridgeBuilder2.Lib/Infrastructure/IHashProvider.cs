using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Infrastructure
{
    /// <summary>
    /// Provides hash generation and comparison.
    /// </summary>
    public interface IHashProvider
    {
        /// <summary>
        /// Compare two hashes and return True if they match.
        /// </summary>
        bool CompareHash(IEnumerable<byte> left, IEnumerable<byte> right);
        
        /// <summary>
        /// Hash some data.
        /// </summary>
        byte[] GetHash(IEnumerable<byte> data);
    }
}