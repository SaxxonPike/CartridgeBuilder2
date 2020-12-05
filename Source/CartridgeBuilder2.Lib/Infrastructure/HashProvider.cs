using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;

namespace CartridgeBuilder2.Lib.Infrastructure
{
    /// <inheritdoc />
    [Service]
    public class HashProvider : IHashProvider
    {
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly int _hashSize;

        public HashProvider()
        {
            // Ordinarily we would just use HashAlgorithm.Create, but this does not exist
            // in every framework we target. So, SHA256 is chosen as an arbitrary default.
            // Really, it does not matter which algo we choose, so long as it's consistent
            // within the context of execution.
            _hashAlgorithm = SHA256.Create();
            _hashSize = _hashAlgorithm.HashSize / 8;
        }

        /// <inheritdoc />
        public bool CompareHash(IEnumerable<byte> left, IEnumerable<byte> right)
        {
            var leftList = left.AsArray();
            var rightList = right.AsArray();

            return leftList.Length == _hashSize && 
                   rightList.Length == _hashSize &&
                   leftList.SequenceEqual(rightList);
        }

        /// <inheritdoc />
        public byte[] GetHash(IEnumerable<byte> data)
        {
            return _hashAlgorithm.ComputeHash(data.AsArray());
        }
    }
}