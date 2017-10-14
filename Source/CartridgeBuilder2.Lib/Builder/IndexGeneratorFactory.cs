using System;
using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc/>
    [Service]
    public class IndexGeneratorFactory : IIndexGeneratorFactory
    {
        /// <inheritdoc/>
        public Func<int, int, IEnumerable<int>> CreateGenerator(WrapStrategy wrapStrategy)
        {
            switch (wrapStrategy)
            {
                case WrapStrategy.Both:
                    return GenerateBothBanksIndices;
                case WrapStrategy.Low:
                    return GenerateLowerBankIndices;
                case WrapStrategy.High:
                    return GenerateUpperBankIndices;
                default:
                    throw new CartridgeBuilderException($"Invalid strategy: {wrapStrategy}");
            }
        }

        private static IEnumerable<int> GenerateBothBanksIndices(int start, int max) =>
            Enumerable.Range(start, max - start);

        private static IEnumerable<int> GenerateLowerBankIndices(int start, int max) =>
            GenerateBothBanksIndices(start, max).Where(i => (i & 0x2000) == 0x0000);

        private static IEnumerable<int> GenerateUpperBankIndices(int start, int max) =>
            GenerateBothBanksIndices(start, max).Where(i => (i & 0x2000) == 0x2000);
        
    }
}