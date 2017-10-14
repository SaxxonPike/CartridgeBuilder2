namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Generates IAllocation objects from other data types such as ITable and IPatch.
    /// </summary>
    public interface IAllocationGenerator
    {
        IAllocation GenerateFromTable(ITable table);
        IAllocation GenerateFromPatch(IPatch patch);
    }
}