using System.Collections.Generic;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc />
    [Service]
    public class PatchWriter : IPatchWriter
    {
        private readonly ILogger _logger;
        private readonly IPacker _packer;
        private readonly IAllocationGenerator _allocationGenerator;

        public PatchWriter(ILogger logger, IPacker packer, IAllocationGenerator allocationGenerator)
        {
            _logger = logger;
            _packer = packer;
            _allocationGenerator = allocationGenerator;
        }
        
        /// <inheritdoc />
        public void Write(IRomSpace romSpace, IEnumerable<IPatch> patches)
        {
            foreach (var patch in patches)
            {
                _logger.Debug($"Patching bank 0x{patch.Bank:X} at offset 0x{patch.Offset:X}");
                var fit = _packer.Write(romSpace, patch.Data, _allocationGenerator.GenerateFromPatch(patch), OverwriteRule.Allow);                
                _logger.Debug($"Patch was applied at ROM offset {fit.Offset:X5}");
            }
        }
    }
}