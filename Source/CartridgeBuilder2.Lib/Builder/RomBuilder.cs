using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc/>
    [Service]
    public class RomBuilder : IRomBuilder
    {
        private readonly ILogger _logger;
        private readonly IFilePacker _filePacker;
        private readonly ITableWriter _tableWriter;
        private readonly IPatchWriter _patchWriter;

        public RomBuilder(ILogger logger,
            IFilePacker filePacker,
            ITableWriter tableWriter,
            IPatchWriter patchWriter)
        {
            _logger = logger;
            _filePacker = filePacker;
            _tableWriter = tableWriter;
            _patchWriter = patchWriter;
        }

        /// <inheritdoc/>
        public IRomBuilderResult Build(IRomBuilderConfiguration romBuilderConfiguration)
        {
            _logger.Debug("Validating configuration");
            
            var tables = romBuilderConfiguration.Tables.Select(t =>
                t ?? throw new CartridgeBuilderException("Table cannot be null.")).AsCollection();
            var files = romBuilderConfiguration.Files.Select(f =>
                f ?? throw new CartridgeBuilderException("File cannot be null."))
                .OrderByDescending(f => f.Data?.Length ?? 0)
                .AsCollection();
            var patches = romBuilderConfiguration.Patches.Select(p =>
                p ?? throw new CartridgeBuilderException("Patch cannot be null.")).AsCollection();
            var fills = romBuilderConfiguration.Fills.Select(f =>
                f ?? throw new CartridgeBuilderException("Fill cannot be null."))
                .Select(ConvertFillToPatch)
                .AsCollection();
            
            var rom = new RomSpace(romBuilderConfiguration.Capacity ?? RomBuilderDefaults.Capacity);

            _tableWriter.Reserve(rom, tables);
            _patchWriter.Write(rom, fills.Concat(patches));
            var packedFiles = _filePacker.Pack(rom, files).AsList();
            _tableWriter.Write(rom, tables, packedFiles);
            
            return new RomBuilderResult
            {
                Files = packedFiles,
                Rom = rom
            };
        }

        private IPatch ConvertFillToPatch(IFill fill)
        {
            if (fill.Data == null)
                throw new CartridgeBuilderException("Fill data cannot be null.");
            if (!fill.Data.Any())
                throw new CartridgeBuilderException("Fill data cannot be empty.");
            
            IEnumerable<byte> Generate()
            {
                while (true)
                    foreach (var b in fill.Data)
                        yield return b;
                // ReSharper disable once IteratorNeverReturns
            }
            
            return new Patch
            {
                Data = Generate().Take(fill.Length).ToArray(),
                Bank = fill.Bank,
                Offset = fill.Offset,
                WrapStrategy = fill.WrapStrategy
            };
        }
    }
}