using System;
using System.Collections.Generic;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Generates sequences which contain offsets which adhere to a particular wrap strategy.
    /// </summary>
    public interface IIndexGeneratorFactory
    {
        /// <summary>
        /// Create a sequence generator for the specified wrap strategy.
        /// </summary>
        Func<int, int, IEnumerable<int>> CreateGenerator(WrapStrategy wrapStrategy);
    }
}