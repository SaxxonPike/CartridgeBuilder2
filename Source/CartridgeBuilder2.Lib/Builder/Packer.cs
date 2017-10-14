using System;
using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc />
    [Service]
    public class Packer : IPacker
    {
        private readonly IIndexGeneratorFactory _indexGeneratorFactory;

        public Packer(IIndexGeneratorFactory indexGenerator)
        {
            _indexGeneratorFactory = indexGenerator;
        }
        
        /// <inheritdoc />
        public IAllocation Fit(IRomSpace romSpace, int size)
        {
            if (romSpace == null)
                throw new ArgumentNullException(nameof(romSpace));
            
            return Enum.GetValues(typeof(WrapStrategy))
                .Cast<WrapStrategy>()
                .AsParallel()
                .Select(strategy => Fit(romSpace, size, strategy))
                .Where(result => result != null)
                .OrderBy(result => result.Offset)
                .ThenBy(result => result.WrapStrategy)
                .FirstOrDefault();
        }

        /// <inheritdoc />
        public IAllocation Write(IRomSpace romSpace, IEnumerable<byte> data, IAllocation allocation)
        {
            if (romSpace == null)
                throw new ArgumentNullException(nameof(romSpace));
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (allocation == null)
                throw new ArgumentNullException(nameof(allocation));
            
            var generate = _indexGeneratorFactory.CreateGenerator(allocation.WrapStrategy);
            var workspaceData = romSpace.Data;
            var used = romSpace.Usage;
            var inputOffset = 0;
            var inputData = allocation.Length == null 
                ? data.AsArray() 
                : data.Take(allocation.Length.Value).AsArray();
            var totalLength = allocation.Length ?? inputData.Length;
            var indices = generate(allocation.Offset, workspaceData.Count).Take(totalLength).AsArray();

            if (indices.Length < allocation.Length)
                throw new CartridgeBuilderException($"Not enough space to write {allocation.Length} bytes at {allocation.Offset}");
            
            foreach (var i in indices)
            {
                if (inputOffset < inputData.Length)
                    workspaceData[i] = inputData[inputOffset++];
                used[i] = UsageType.Used;
            }
            
            return new Allocation
            {
                CompressionMethod = allocation.CompressionMethod,
                Length = allocation.Length,
                Offset = indices.First(),
                WrapStrategy = allocation.WrapStrategy
            };
        }

        /// <inheritdoc />
        public IAllocation Reserve(IRomSpace romSpace, IAllocation allocation)
        {
            if (romSpace == null)
                throw new ArgumentNullException(nameof(romSpace));
            if (allocation == null)
                throw new ArgumentNullException(nameof(allocation));
            
            var length = allocation.Length ?? throw new CartridgeBuilderException("Allocation length must not be null");
            var generate = _indexGeneratorFactory.CreateGenerator(allocation.WrapStrategy);
            var used = romSpace.Usage;
            var indices = generate(allocation.Offset, used.Count).Take(length).AsArray();
            
            if (indices.Length < allocation.Length)
                throw new CartridgeBuilderException($"Not enough space to reserve {allocation.Length} bytes at {allocation.Offset}");

            foreach (var i in indices)
                used[i] = UsageType.Reserved;
            
            return new Allocation
            {
                CompressionMethod = allocation.CompressionMethod,
                Length = allocation.Length,
                Offset = indices.First(),
                WrapStrategy = allocation.WrapStrategy
            };
        }

        private IAllocation Fit(IRomSpace romSpace, int size, WrapStrategy strategy)
        {
            var generate = _indexGeneratorFactory.CreateGenerator(strategy);
            var data = romSpace.Data;
            var used = romSpace.Usage;

            return generate(0, data.Count - size)
                .Where(i => generate(i, i + size).All(j => used[j] == UsageType.Unused))
                .Select(i => new Allocation {Offset = i, Length = size, WrapStrategy = strategy})
                .FirstOrDefault();
        }
    }
}