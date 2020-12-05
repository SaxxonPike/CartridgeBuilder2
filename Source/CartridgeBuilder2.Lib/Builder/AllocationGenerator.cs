using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc />
    [Service]
    public class AllocationGenerator : IAllocationGenerator
    {
        /// <inheritdoc />
        public IAllocation GenerateFromTable(ITable table) => new Allocation
        {
            Offset = table.Bank * 0x4000 + table.Offset,
            WrapStrategy = table.WrapStrategy,
            Length = table.Length
        };

        /// <inheritdoc />
        public IAllocation GenerateFromPatch(IPatch patch) => new Allocation
        {
            Offset = patch.Bank * 0x4000 + patch.Offset,
            Length = patch.Data.Length,
            WrapStrategy = patch.WrapStrategy
        };
    }
}