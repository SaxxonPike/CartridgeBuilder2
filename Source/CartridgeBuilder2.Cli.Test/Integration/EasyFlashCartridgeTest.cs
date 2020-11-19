using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using CartridgeBuilder2.Cli.Configuration;
using CartridgeBuilder2.Lib.Builder;
using NUnit.Framework;

namespace CartridgeBuilder2.Cli.Test.Integration
{
    [TestFixture]
    public class EasyFlashCartridgeTest : BaseCliIntegrationFixture
    {
        [Test]
        public void CompileEasyFlashCartridge()
        {
            // Arrange.
            var fileName = Create<string>();
            WriteFile(fileName, CreateMany<byte>());

            var outputName = Create<string>();
            var configName = Create<string>();
            WriteConfig(configName, new BuilderConfig
            {
                Files = Build<FileConfig>().With(x => x.Path, fileName).CreateMany().ToArray(),
                Tables = new List<TableConfig>
                {
                    new TableConfig
                    {
                        Bank = 0,
                        Type = TableType.StartAddressLow,
                        Offset = 0x0000,
                        Length = 0x20
                    },
                    new TableConfig
                    {
                        Bank = 0,
                        Type = TableType.StartAddressHigh,
                        Offset = 0x0020,
                        Length = 0x20
                    },
                    new TableConfig
                    {
                        Bank = 0,
                        Type = TableType.Bank,
                        Offset = 0x0040,
                        Length = 0x20
                    }
                },
                OutputFile = outputName
            });

            // Act.
            Subject.Run(new List<string> {configName});

            // Assert.
            var cart = FileSystem.ReadAllBytes(outputName);
//            File.WriteAllBytes(
//                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
//                    $"{DateTimeOffset.Now:yyyyMMdd-HHmmss}.crt"), cart);
        }
    }
}