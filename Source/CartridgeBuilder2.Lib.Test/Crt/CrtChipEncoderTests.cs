using System;
using System.IO;
using System.Linq;
using CartridgeBuilder2.Lib.Crt;
using CartridgeBuilder2.Lib.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Crt
{
    [TestFixture]
    public class CrtChipEncoderTests : BaseUnitTestFixture<CrtChipEncoder, ICrtChipEncoder>
    {
        [SetUp]
        public void SetupImplementations()
        {
            Mocker.Implement<IByteSwapper>(new ByteSwapper());
        }

        [Test]
        public void Encode_ThrowsIfChipIsNull()
        {
            // Arrange.
            var chip = new Chip();

            // Act.
            Action act = () => Subject.Encode(null, chip);

            // Assert.
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Encode_ThrowsIfStreamIsNull()
        {
            // Arrange.
            var mem = new MemoryStream();

            // Act.
            Action act = () => Subject.Encode(mem, null);

            // Assert.
            act.Should().Throw<ArgumentNullException>();
        }

        [Test]
        public void Encode_WritesCorrectBytes(
            [Random(0x0000, 0xFFFF, 1)] int expectedAddress,
            [Random(0x0000, 0xFFFF, 1)] int expectedBank,
            [Random(0x0000, 0x0002, 1)] int expectedType,
            [Values(0x0001, 0x0010)] int romSize,
            [Values(0x0000, 0x0001, 0x0010)] int extraDataSize
        )
        {
            // Arrange.
            var expectedExtraData = CreateMany<byte>(extraDataSize);
            var expectedRom = CreateMany<byte>(romSize);
            var mem = new MemoryStream();
            var headerLength = romSize + extraDataSize + 0x10;
            var chip = new Chip
            {
                Address = expectedAddress,
                Bank = expectedBank,
                ExtraData = expectedExtraData,
                Rom = expectedRom,
                Type = expectedType
            };

            // Act.
            Subject.Encode(mem, chip);
            mem.Flush();

            // Assert.
            mem.ToArray().Should().BeEquivalentTo(new byte[]
            {
                0x43, 0x48, 0x49, 0x50,
                unchecked((byte) (headerLength >> 24)), unchecked((byte) (headerLength >> 16)),
                unchecked((byte) (headerLength >> 8)), unchecked((byte) headerLength),
                unchecked((byte) (expectedType >> 8)), unchecked((byte) expectedType),
                unchecked((byte) (expectedBank >> 8)), unchecked((byte) expectedBank),
                unchecked((byte) (expectedAddress >> 8)), unchecked((byte) expectedAddress),
                unchecked((byte) (romSize >> 8)), unchecked((byte) romSize)
            }.Concat(expectedExtraData).Concat(expectedRom).ToArray());
        }
    }
}