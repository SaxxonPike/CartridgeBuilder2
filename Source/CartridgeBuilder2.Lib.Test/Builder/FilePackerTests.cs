using System.Collections.Generic;
using System.Linq;
using CartridgeBuilder2.Lib.Builder;
using CartridgeBuilder2.Lib.Infrastructure;
using CartridgeBuilder2.Lib.Prg;
using FluentAssertions;
using Moq;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Builder
{
    [TestFixture]
    public class FilePackerTests : BaseUnitTestFixture<FilePacker, IFilePacker>
    {
        [SetUp]
        public void SetupMocks()
        {
            Mocker.Implement<IHashProvider>(new HashProvider());
            Mocker.Implement<IAddressGenerator>(new AddressGenerator());
        }
        
        [Test]
        public void Pack_PacksSingleFile()
        {
            // Arrange.
            var romSpace = new RomSpace(0x100);
            var files = CreateMany<File>(1).ToArray();
            var fitOutput = Create<Allocation>();
            var writeOutput = Create<Allocation>();

            Mocker.Mock<IPacker>(mock =>
            {
                mock.Setup(x => x.Fit(It.IsAny<IRomSpace>(), It.IsAny<int>()))
                    .Returns(fitOutput);
                mock.Setup(x => x.Write(It.IsAny<IRomSpace>(), It.IsAny<IEnumerable<byte>>(), It.IsAny<IAllocation>()))
                    .Returns(writeOutput);
            });

            // Act.
            var output = Subject.Pack(romSpace, files).Single();

            // Assert.
            output.CompressionMethod.Should().Be(writeOutput.CompressionMethod);
            output.Length.Should().Be(writeOutput.Length);
            output.LoadAddress.Should().Be(files[0].LoadAddress);
            output.Name.Should().BeEquivalentTo(files[0].Name);
            output.StartAddress.Should().Be((writeOutput.Offset & 0x3FFF) | 0x8000);
            output.StartBank.Should().Be((writeOutput.Offset) >> 14);
            output.WrapStrategy.Should().Be(writeOutput.WrapStrategy);
        }
        
        
    }
}