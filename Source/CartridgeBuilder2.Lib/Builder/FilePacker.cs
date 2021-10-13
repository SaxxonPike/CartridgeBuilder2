using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;
using CartridgeBuilder2.Lib.Prg;

namespace CartridgeBuilder2.Lib.Builder
{
    /// <inheritdoc />
    [Service(typeof(IFilePacker))]
    public class FilePacker : IFilePacker
    {
        private readonly ILogger _logger;
        private readonly IHashProvider _hashProvider;
        private readonly IPacker _packer;
        private readonly IAddressGenerator _addressGenerator;
        private readonly IStringConverter _stringConverter;

        public FilePacker(
            ILogger logger, 
            IHashProvider hashProvider, 
            IPacker packer,
            IAddressGenerator addressGenerator,
            IStringConverter stringConverter)
        {
            _logger = logger;
            _hashProvider = hashProvider;
            _packer = packer;
            _addressGenerator = addressGenerator;
            _stringConverter = stringConverter;
        }

        /// <inheritdoc />
        public IEnumerable<IPackedFile> Pack(IRomSpace romSpace, IEnumerable<IFile> files)
        {
            var outFiles = new List<IPackedFile>();
            var outHashes = new Dictionary<byte[], IPackedFile>();
            
            foreach (var file in files)
            {
                _logger.Info($"Packing {_stringConverter.ConvertToString(file.Name)}");
                var loadAddress = file.LoadAddress;
                var fileData = file.Data.AsArray();
                if (loadAddress == null)
                {
                    if (fileData.Length < 2)
                        throw new CartridgeBuilderException("Can't determine load address of a file less than two bytes large");
                    
                    loadAddress = fileData[0] | (fileData[1] << 8);
                    fileData = fileData.Skip(2).AsArray();
                    _logger.Debug($"No load address, first two bytes gave value {loadAddress:X4}");
                }

                var fileHash = file.Dedupe
                    ? _hashProvider.GetHash(fileData).AsArray()
                    : null;

                if (file.Dedupe)
                {
                    var existingHash = outHashes.Keys.FirstOrDefault(h => _hashProvider.CompareHash(h, fileHash));
                    if (existingHash != null)
                    {
                        var existingFile = outHashes[existingHash];
                        _logger.Debug($"Deduplicating file at {existingFile.StartBank:X2}:{existingFile.StartAddress:X4}");
                    
                        // Deduping copies all data except filename. This needs to be preserved.
                        outFiles.Add(new PackedFile
                        {
                            CompressionMethod = existingFile.CompressionMethod,
                            Length = existingFile.Length,
                            LoadAddress = loadAddress,
                            Name = file.Name.ToArray(),
                            StartAddress = existingFile.StartAddress,
                            StartBank = existingFile.StartBank,
                            WrapStrategy = existingFile.WrapStrategy
                        });

                        continue;
                    }                    
                }
                
                var fit = _packer.Fit(romSpace, fileData.Length);
                if (fit == null)
                    throw new CartridgeBuilderException($"Can't fit {fileData.Length} bytes.");

                _logger.Debug($"Packing file at ROM offset {fit.Offset:X5} using wrap strategy {fit.WrapStrategy} and compression {fit.CompressionMethod}");
                var placement = _packer.Write(romSpace, fileData, fit, OverwriteRule.Deny);
                var newFile = new PackedFile
                {
                    CompressionMethod = placement.CompressionMethod,
                    Length = placement.Length ?? 0,
                    LoadAddress = loadAddress,
                    Name = file.Name.ToArray(),
                    StartAddress = _addressGenerator.GetAddress(placement.Offset),
                    StartBank = _addressGenerator.GetBank(placement.Offset),
                    WrapStrategy = placement.WrapStrategy
                };
                outFiles.Add(newFile);
                
                if (file.Dedupe)
                    outHashes[fileHash] = newFile;
                
            }

            return outFiles;
        }
    }
}