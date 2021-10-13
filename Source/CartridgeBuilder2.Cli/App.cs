using System.Collections.Generic;
using System.IO;
using System.Linq;
using CartridgeBuilder2.Cli.Configuration;
using CartridgeBuilder2.Cli.Infrastructure;
using CartridgeBuilder2.Lib.Builder;
using CartridgeBuilder2.Lib.Crt;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Cli
{
    [Service(typeof(IApp))]
    public class App : IApp
    {
        private readonly IConfigurationImporter _configurationImporter;
        private readonly IConfigurationMapper _configurationMapper;
        private readonly ICrtFileEncoder _crtFileEncoder;
        private readonly IConfigCrtExporter _configCrtExporter;
        private readonly IRomChipExporter _romChipExporter;
        private readonly ITimekeeper _timekeeper;
        private readonly IFileSystem _fileSystem;
        private readonly ILogger _logger;
        private readonly IRomBuilder _romBuilder;

        public App(
            ILogger logger,
            IConfigurationImporter configurationImporter,
            IFileSystem fileSystem,
            IRomBuilder romBuilder,
            IConfigurationMapper configurationMapper,
            ICrtFileEncoder crtFileEncoder,
            IConfigCrtExporter configCrtExporter,
            IRomChipExporter romChipExporter,
            ITimekeeper timekeeper)
        {
            _logger = logger;
            _configurationImporter = configurationImporter;
            _fileSystem = fileSystem;
            _romBuilder = romBuilder;
            _configurationMapper = configurationMapper;
            _crtFileEncoder = crtFileEncoder;
            _configCrtExporter = configCrtExporter;
            _romChipExporter = romChipExporter;
            _timekeeper = timekeeper;
        }

        public void Run(IList<string> args)
        {
            LogIntro();

            if (args.Count < 1)
            {
                LogUsage();
                return;
            }

            _logger.Info($"Loading config {args[0]}");
            _timekeeper.Start();
            var config = GetConfig(args[0]);

            var outputPath = args.Count >= 2
                ? args[1]
                : (string.IsNullOrWhiteSpace(config.OutputFile) ? "output.crt" : config.OutputFile);
            _logger.Info($"Output file is {outputPath}");

            _logger.Info("Processing config");
            var mappedConfig = _configurationMapper.MapRomBuilderConfiguration(config);
            
            _timekeeper.Stop();
            _logger.Info($"Crunched config in {_timekeeper.Elapsed.TotalMilliseconds}ms");
            _timekeeper.Reset();
            
            _logger.Info("Packing files");
            _timekeeper.Start();
            var packed = _romBuilder.Build(mappedConfig);

            using (var outStream = _fileSystem.OpenWrite(outputPath + ".raw"))
            {
                _logger.Info($"Writing {outputPath}.raw");
                var writer = new BinaryWriter(outStream);
                writer.Write(packed.Rom.Data);
            }

            _logger.Info("Building chips");
            var chips = _romChipExporter.Export(packed.Rom);

            _logger.Info("Assembling CRT");
            var crt = _configCrtExporter.Export(config, chips);
            
            _timekeeper.Stop();
            _logger.Info($"Crunched CRT in {_timekeeper.Elapsed.TotalMilliseconds}ms");
            
            using (var outStream = _fileSystem.OpenWrite(outputPath))
            {
                _logger.Info($"Writing {outputPath}");
                _crtFileEncoder.Encode(outStream, crt);
            }
        }

        private BuilderConfig GetConfig(string path)
        {
            using (var stream = _fileSystem.OpenRead(path))
            {
                return _configurationImporter.Import(stream);
            }
        }

        private void LogIntro()
        {
            _logger.Info("CartridgeBuilder CLI");
            _logger.Info(string.Empty);
        }

        private void LogUsage()
        {
            _logger.Info("Usage:");
            _logger.Info("  cb <configfile> [<outputfile>]");
        }
    }
}