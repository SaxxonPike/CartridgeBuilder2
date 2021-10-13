using System.Collections.Generic;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc />
    [Service(typeof(ITableWriter))]
    public class TableWriter : ITableWriter
    {
        private readonly ILogger _logger;
        private readonly IAllocationGenerator _allocationGenerator;
        private readonly ITableBuilder _tableBuilder;
        private readonly IPacker _packer;

        public TableWriter(
            ILogger logger,
            IAllocationGenerator allocationGenerator,
            ITableBuilder tableBuilder,
            IPacker packer)
        {
            _logger = logger;
            _allocationGenerator = allocationGenerator;
            _tableBuilder = tableBuilder;
            _packer = packer;
        }
        
        /// <inheritdoc />
        public void Reserve(IRomSpace romSpace, IEnumerable<ITable> tables)
        {
            foreach (var table in tables)
            {
                _logger.Debug($"Reserving table in bank 0x{table.Bank:X} at offset 0x{table.Offset:X}");
                var fit = _packer.Reserve(romSpace, _allocationGenerator.GenerateFromTable(table));
                _logger.Debug($"Table was reserved at ROM offset {fit.Offset:X5}");
            }
        }

        /// <inheritdoc />
        public void Write(IRomSpace romSpace, IEnumerable<ITable> tables, IEnumerable<IPackedFile> packedFiles)
        {
            var files = packedFiles.AsArray();
            foreach (var table in tables)
            {
                _logger.Debug($"Building table of type {table.Type}");
                var data = _tableBuilder.Build(table, files);
                _logger.Debug($"Patching table in bank 0x{table.Bank:X} at offset 0x{table.Offset:X}");
                var fit = _packer.Write(romSpace, data, _allocationGenerator.GenerateFromTable(table), OverwriteRule.Allow);
                _logger.Debug($"Table was patched at ROM offset {fit.Offset:X5}");
            }
        }
    }
}