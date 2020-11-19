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
    public class CrtChipDecoderTests : BaseUnitTestFixture<CrtChipDecoder, ICrtChipDecoder>
    {
        [SetUp]
        public void SetupImplementations()
        {
            Mocker.Implement<IByteSwapper>(new ByteSwapper());
        }

        [Theory]
        public void Decode_ReadsCorrectValues(
            [Values(0x8000, 0xA000)] int expectedAddress,
            [Random(0x00, 0xFF, 2)] int expectedBank,
            [Values(0x0010, 0x0100)] int expectedLength,
            [Range(0x0000, 0x0002)] int expectedType,
            [Values(0x0000, 0x0010)] int expectedExtraDataLength)
        {
            // Arrange.
            var data = CreateMany<byte>(expectedLength);
            var extraData = CreateMany<byte>(expectedExtraDataLength);
            var headerLength = expectedLength + 0x10 + expectedExtraDataLength;
            var header = new byte[]
            {
                0x43, 0x48, 0x49, 0x50,
                unchecked((byte) (headerLength >> 24)), unchecked((byte) (headerLength >> 16)),
                unchecked((byte) (headerLength >> 8)), unchecked((byte) headerLength),
                unchecked((byte) (expectedType >> 8)), unchecked((byte) expectedType),
                unchecked((byte) (expectedBank >> 8)), unchecked((byte) expectedBank),
                unchecked((byte) (expectedAddress >> 8)), unchecked((byte) expectedAddress),
                unchecked((byte) (expectedLength >> 8)), unchecked((byte) expectedLength)
            };
            var chip = header.Concat(extraData).Concat(data).ToArray();
            var mem = new MemoryStream(chip);

            // Act.
            var output = Subject.Decode(mem);

            // Assert.
            output.Address.Should().Be(expectedAddress, $"{nameof(IChip.Address)} must be {expectedAddress:X4}");
            output.Bank.Should().Be(expectedBank, $"{nameof(IChip.Bank)} must be {expectedBank:X4}");
            output.Type.Should().Be(expectedType, $"{nameof(IChip.Type)} must be {expectedType:X4}");
            output.ExtraData.Should()
                .AllBeEquivalentTo(extraData, $"{nameof(IChip.ExtraData)} must have correct contents");
            output.Rom.Should().AllBeEquivalentTo(data, $"{nameof(IChip.Rom)} must have correct contents");
        }

        [Test]
        public void Decode_ReturnsNullWhenNoDataIsPresent()
        {
            // Arrange.
            var mem = new MemoryStream();

            // Act.
            var output = Subject.Decode(mem);

            // Assert.
            output.Should().BeNull();
        }

        [Test]
        public void Decode_ThrowsOnInvalidId()
        {
            // Arrange.
            var data = CreateMany<byte>(0x10);
            var mem = new MemoryStream(data);
            var chipId = data[0] | (data[1] << 8) | (data[2] << 16) | (data[3] << 24);

            // Act.
            Action act = () => Subject.Decode(mem);

            // Assert.
            act.Should().Throw<CartridgeBuilderException>()
                .WithMessage($"Chip ID is invalid. Found: {chipId:X8}");
        }

        [Test]
        public void Decode_ThrowsOnInvalidSize(
            [Random(0x00, 0x0F, 1)] byte value)
        {
            // Arrange.
            var header = new byte[]
            {
                0x43, 0x48, 0x49, 0x50,
                0x00, 0x00, 0x00, value,
                0x00, 0x00, 0x00, 0x00,
                0x00, 0x00, 0x00, 0x00
            };
            var mem = new MemoryStream(header);

            // Act.
            Action act = () => Subject.Decode(mem);

            // Assert.
            act.Should().Throw<CartridgeBuilderException>()
                .WithMessage($"Chip header length is invalid. Found: {value:X8}");
        }

        [Test]
        public void Decode_ThrowsOnPartialHeader()
        {
            // Arrange.
            var data = CreateMany<byte>(0x04).ToArray();
            var mem = new MemoryStream(data);

            // Act.
            Action act = () => Subject.Decode(mem);

            // Assert.
            act.Should().Throw<CartridgeBuilderException>()
                .WithMessage($"Chip header is less than 16 bytes. Found only {data.Length} bytes");
        }

        [Test]
        public void Decode_ThrowsWithNullParameter()
        {
            // Act.
            Action act = () => Subject.Decode(null);

            // Assert.
            act.Should().Throw<ArgumentNullException>();
        }
    }
}