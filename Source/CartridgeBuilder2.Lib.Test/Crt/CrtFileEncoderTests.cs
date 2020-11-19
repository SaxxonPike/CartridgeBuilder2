using System.IO;
using System.Linq;
using AutoFixture;
using CartridgeBuilder2.Lib.Crt;
using Moq;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Crt
{
    [TestFixture]
    public class CrtFileEncoderTests : BaseUnitTestFixture<CrtFileEncoder, ICrtFileEncoder>
    {
        [Test]
        public void Encode_CallsEncoderFunctions()
        {
            // Arrange.
            var crt = Build<CrtFile>()
                .With(x => x.Info, Create<CrtInfo>())
                .With(x => x.Chips, CreateMany<Chip>().Cast<IChip>().ToList())
                .Create();
            var mem = new MemoryStream();

            // Act.
            Subject.Encode(mem, crt);

            // Assert.
            Mock<ICrtInfoEncoder>().Verify(x => x.Encode(It.IsAny<Stream>(), crt.Info));
            foreach (var chip in crt.Chips)
                Mock<ICrtChipEncoder>().Verify(x => x.Encode(It.IsAny<Stream>(), chip));
        }
    }
}