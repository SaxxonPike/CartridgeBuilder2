namespace CartridgeBuilder2.Lib.Builder
{
    /// <summary>
    /// Compiles files, patches and tables into a flat ROM image.
    /// </summary>
    public interface IRomBuilder
    {
        /// <summary>
        /// Compile the specified configuration into a flat ROM image.
        /// </summary>
        IRomBuilderResult Build(IRomBuilderConfiguration romBuilderConfiguration);
    }
}