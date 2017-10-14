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
                t ?? throw new CartridgeBuilderException("Table cannot be null.")).AsArray();
            var files = romBuilderConfiguration.Files.Select(f =>
                f ?? throw new CartridgeBuilderException("File cannot be null.")).AsArray();
            var patches = romBuilderConfiguration.Patches.Select(p =>
                p ?? throw new CartridgeBuilderException("Patch cannot be null.")).AsArray();
            var rom = new RomSpace(romBuilderConfiguration.Capacity ?? RomBuilderDefaults.Capacity);

            _tableWriter.Reserve(rom, tables);
            _patchWriter.Write(rom, patches);
            var packedFiles = _filePacker.Pack(rom, files).AsArray();
            _tableWriter.Write(rom, tables, packedFiles);
            
            return new RomBuilderResult
            {
                Files = packedFiles,
                Rom = rom
            };
        }
    }
}