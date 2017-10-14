using System.IO;
using System.Text;
using CartridgeBuilder2.Cli.Configuration;
using CartridgeBuilder2.Lib.Builder;
using CartridgeBuilder2.Lib.Test;
using FluentAssertions;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CartridgeBuilder2.Cli.Test.Configuration
{
    [TestFixture]
    public class RomBuilderConfigImporterTests : BaseUnitTestFixture<ConfigurationImporter, IConfigurationImporter>
    {
        private static Stream TextStream(object d)
        {
            var json = JsonConvert.SerializeObject(d);
            TestContext.WriteLine(json);
            return new MemoryStream(Encoding.UTF8.GetBytes(json));
        }

        [Test]
        public void Import_SetsConfigDefaults()
        {
            // Arrange.
            var config = TextStream(new
            {
                Files = new object[0],
                Partitions = new object[0]
            });

            // Act.
            var output = Subject.Import(config);

            // Assert.
            output.Files.Should().BeEmpty();
            output.Patches.Should().BeEmpty();
            output.Tables.Should().BeEmpty();
            output.Capacity.Should().BeNull();
        }

        [Test]
        public void Import_SetsConfigValues()
        {
            // Arrange.
            var outputFile = Create<string>();
            var config = TextStream(new
            {
                Files = new object[0],
                Partitions = new object[0],
                OutputFile = outputFile
            });

            // Act.
            var output = Subject.Import(config);

            // Assert.
            output.OutputFile.Should().Be(outputFile);
        }

        [Test]
        public void Import_SetsFileDefaults()
        {
            // Arrange.
            var path = Create<string>();
            var config = TextStream(new
            {
                Files = new[]
                {
                    new {Path = path}
                },
                Partitions = new string[0]
            });

            // Act.
            var output = Subject.Import(config);

            // Assert.
            output.Files.Should().HaveCount(1);
            output.Files[0].LoadAddress.Should().BeNull();
            output.Files[0].Name.Should().BeNull();
        }

        [Test]
        public void Import_SetsFileValues()
        {
            // Arrange.
            var path = Create<string>();
            var name = Create<string>();
            var loadAddress = Create<int>();

            var config = TextStream(new
            {
                Files = new[]
                {
                    new {Path = path, Name = name, LoadAddress = loadAddress}
                },
                Partitions = new string[0]
            });

            // Act.
            var output = Subject.Import(config);

            // Assert.
            output.Files.Should().HaveCount(1);
            output.Files[0].Path.Should().Be(path);
            output.Files[0].LoadAddress.Should().Be(loadAddress);
            output.Files[0].Name.Should().Be(name);
        }

        [Test]
        public void Import_SetsPatchDefaults()
        {
            // Arrange.
            var bank = Create<int>();
            var offset = Create<int>();
            var path = Create<string>();
            var config = TextStream(new
            {
                Files = new object[0],
                Partitions = new object[0],
                Patches = new[]
                {
                    new {Bank = bank, Offset = offset, Path = path}
                }
            });

            // Act.
            var output = Subject.Import(config);

            // Assert.
            output.Patches.Should().HaveCount(1);
            output.Patches[0].WrapStrategy.Should().Be(WrapStrategy.Both);
        }

        [Test]
        public void Import_SetsPatchValues()
        {
            // Arrange.
            var bank = Create<int>();
            var offset = Create<int>();
            var path = Create<string>();
            var usage = Create<WrapStrategy>();
            var config = TextStream(new
            {
                Files = new object[0],
                Partitions = new object[0],
                Patches = new[]
                {
                    new {Bank = bank, Offset = offset, Path = path, Usage = usage.ToString()}
                }
            });

            // Act.
            var output = Subject.Import(config);

            // Assert.
            output.Patches.Should().HaveCount(1);
            output.Patches[0].Bank.Should().Be(bank);
            output.Patches[0].Offset.Should().Be(offset);
            output.Patches[0].Path.Should().Be(path);
            output.Patches[0].WrapStrategy.Should().Be(usage);
        }

        [Test]
        public void Import_SetsTableDefaults()
        {
            // Arrange.
            var bank = Create<int>();
            var offset = Create<int>();
            var type = Create<TableType>();
            var config = TextStream(new
            {
                Files = new object[0],
                Partitions = new object[0],
                Tables = new[]
                {
                    new {Bank = bank, Offset = offset, Type = type}
                }
            });

            // Act.
            var output = Subject.Import(config);

            // Assert.
            output.Tables.Should().HaveCount(1);
            output.Tables[0].Index.Should().Be(0);
            output.Tables[0].Usage.Should().Be(WrapStrategy.Both);
        }

        [Test]
        public void Import_SetsTableValues()
        {
            // Arrange.
            var bank = Create<int>();
            var offset = Create<int>();
            var type = Create<TableType>();
            var index = Create<int>();
            var usage = Create<WrapStrategy>();
            var config = TextStream(new
            {
                Files = new object[0],
                Partitions = new object[0],
                Tables = new[]
                {
                    new {Bank = bank, Offset = offset, Type = type, Index = index, Usage = usage.ToString()}
                }
            });

            // Act.
            var output = Subject.Import(config);

            // Assert.
            output.Tables.Should().HaveCount(1);
            output.Tables[0].Bank.Should().Be(bank);
            output.Tables[0].Index.Should().Be(index);
            output.Tables[0].Offset.Should().Be(offset);
            output.Tables[0].Type.Should().Be(type);
            output.Tables[0].Usage.Should().Be(usage);
        }
    }
}