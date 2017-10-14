using System.IO;
using CartridgeBuilder2.Lib.Crt;
using Moq;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Crt
{
    [TestFixture]
    public class CrtFileDecoderTests : BaseUnitTestFixture<CrtFileDecoder, ICrtFileDecoder>
    {
        [Test]
        public void Decode_CallsDecodeForAllChunks()
        {
            // Arrange.
            var mem = new MemoryStream();
            var chips = new[]
            {
                new Chip(),
                new Chip(),
                null
            };

            Mock<ICrtChipDecoder>(mock =>
            {
                var sequence = mock.SetupSequence(x => x.Decode(It.IsAny<Stream>()));
                foreach (var chip in chips)
                    sequence.Returns(chip);
            });

            // Act.
            Subject.Decode(mem);

            // Assert.
            Mock<ICrtInfoDecoder>().Verify(x => x.Decode(It.IsAny<Stream>()));
            Mock<ICrtChipDecoder>().Verify(x => x.Decode(It.IsAny<Stream>()), Times.Exactly(chips.Length));
        }
    }
}