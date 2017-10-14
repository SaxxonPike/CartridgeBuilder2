namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Describes geometry for placement within a ROM.
    /// </summary>
    public interface IAllocation
    {
        CompressionMethod CompressionMethod { get; }
        int Offset { get; }
        int? Length { get; }
        WrapStrategy WrapStrategy { get; }
    }
}