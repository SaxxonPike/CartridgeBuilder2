using System.Linq;
using CartridgeBuilder2.Cli.Infrastructure;
using CartridgeBuilder2.Lib.Builder;
using CartridgeBuilder2.Lib.Infrastructure;
using CartridgeBuilder2.Lib.Prg;

namespace CartridgeBuilder2.Cli.Configuration
{
    public class ConfigurationMapper : IConfigurationMapper
    {
        private readonly IFileSystem _fileSystem;
        private readonly IStringConverter _stringConverter;

        public ConfigurationMapper(IFileSystem fileSystem, IStringConverter stringConverter)
        {
            _fileSystem = fileSystem;
            _stringConverter = stringConverter;
        }

        public IRomBuilderConfiguration MapRomBuilderConfiguration(BuilderConfig config)
        {
            return new RomBuilderConfiguration
            {
                Files = config.Files.SelectMany(f => _fileSystem.Expand(f.Path).Select(fn => new File
                {
                    Name = _stringConverter.ConvertToBytes(_fileSystem.GetName(fn)),
                    LoadAddress = f.LoadAddress,
                    Data = _fileSystem.ReadAllBytes(fn),
                    Dedupe = f.Dedupe
                })).ToList<IFile>(),
                Patches = config.Patches.Select(p => new Patch
                {
                    Bank = p.Bank,
                    Data = _fileSystem.ReadAllBytes(p.Path),
                    Offset = p.Offset,
                    WrapStrategy = p.WrapStrategy,
                    Dedupe = p.Dedupe
                }).ToList<IPatch>(),
                Tables = config.Tables.Select(t => new Table
                {
                    Bank = t.Bank,
                    Index = t.Index,
                    Length = t.Length ?? config.Files.Count,
                    Offset = t.Offset,
                    Type = t.Type
                }).ToList<ITable>(),
                Fills = config.Fills.Select(f => new Fill
                {
                    Bank = f.Bank,
                    Data = f.Bytes.ToList(),
                    Length = f.Length,
                    Offset = f.Offset,
                    WrapStrategy = f.WrapStrategy,
                    Dedupe = f.Dedupe
                }).ToList<IFill>()
            };
        }
    }
}