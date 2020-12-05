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
                .Select(strategy => Fit(romSpace, size, strategy))
                .Where(result => result != null)
                .OrderBy(result => result.Offset)
                .ThenBy(result => result.WrapStrategy)
                .FirstOrDefault();
        }

        /// <inheritdoc />
        public IAllocation Write(IRomSpace romSpace, byte[] data, IAllocation allocation, OverwriteRule overwriteRule)
        {
            if (romSpace == null)
                throw new ArgumentNullException(nameof(romSpace));
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (allocation == null)
                throw new ArgumentNullException(nameof(allocation));

            var span = data.AsSpan();
            var generate = _indexGeneratorFactory.CreateGenerator(allocation.WrapStrategy);
            var workspaceData = romSpace.Data;
            var used = romSpace.Usage;
            var inputOffset = 0;
            var inputData = allocation.Length == null 
                ? span
                : span.Slice(0, allocation.Length.Value);
            var totalLength = allocation.Length ?? inputData.Length;
            var indices = generate(allocation.Offset, workspaceData.Length).Take(totalLength).AsArray();

            if (indices.Length < allocation.Length)
                throw new CartridgeBuilderException($"Not enough space to write {allocation.Length} bytes at {allocation.Offset}");

            if (overwriteRule == OverwriteRule.Allow)
            {
                foreach (var i in indices)
                {
                    if (inputOffset < inputData.Length)
                        workspaceData[i] = inputData[inputOffset++];

                    used[i] = UsageType.Used;
                    if (i >= romSpace.DataContentLength)
                        romSpace.DataContentLength = i + 1;
                }                
            }
            else
            {
                foreach (var i in indices)
                {
                    if (inputOffset < inputData.Length)
                        workspaceData[i] = inputData[inputOffset++];

                    if (used[i] == UsageType.Used)
                        throw new CartridgeBuilderException($"Overwritten data at {allocation.Offset}");
                    
                    used[i] = UsageType.Used;
                    if (i >= romSpace.DataContentLength)
                        romSpace.DataContentLength = i + 1;
                }                
            }
            
            for (var i = romSpace.LowestAvailable; i <= romSpace.DataContentLength; i++)
            {
                if (used[i] != UsageType.Unused) 
                    continue;
                romSpace.LowestAvailable = i;
                break;
            }
            
            return new Allocation
            {
                CompressionMethod = allocation.CompressionMethod,
                Length = allocation.Length,
                Offset = allocation.Length < 1 ? 0 : indices.First(),
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
            {
                if (i >= romSpace.DataContentLength)
                    romSpace.DataContentLength = i + 1;
                used[i] = UsageType.Reserved;
            }

            for (var i = romSpace.LowestAvailable; i <= romSpace.DataContentLength; i++)
            {
                if (used[i] != UsageType.Unused) 
                    continue;
                romSpace.LowestAvailable = i;
                break;
            }
            
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
            var used = romSpace.Usage;
            var minimum = romSpace.LowestAvailable;

            if (size == 0)
                return new Allocation {Offset = 0, Length = 0, WrapStrategy = WrapStrategy.Both};
            
            return generate(minimum, romSpace.Data.Length - size)
                .Where(i =>
                {
                    if (i < minimum)
                        return false;
                    if (used[i] != UsageType.Unused)
                        return false;
                    
                    var check = generate(i + 1, romSpace.Data.Length)
                        .Take(size - 1)
                        .TakeWhile(j => used[j] == UsageType.Unused)
                        .AsCollection();

                    if (check.Count < size - 1)
                    {
                        if (check.Count > 0)
                            minimum = check.Last() + 1;
                        return false;
                    }

                    return true;
                })
                .Select(i => new Allocation {Offset = i, Length = size, WrapStrategy = strategy})
                .FirstOrDefault();
        }
    }
}