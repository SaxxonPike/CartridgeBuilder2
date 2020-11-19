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
    public class CrtInfoDecoderTests : BaseUnitTestFixture<CrtInfoDecoder, ICrtInfoDecoder>
    {
        [SetUp]
        public void SetupImplementations()
        {
            _stringConverter = new StringConverter();
            Mocker.Implement(_stringConverter);
            Mocker.Implement<IByteSwapper>(new ByteSwapper());
        }

        private IStringConverter _stringConverter;

        [Test]
        public void Decode_DecodesStream(
            [Values(0x0000, 0x0010)] int extraDataLength,
            [Random(0x0000, 0xFFFF, 1)] int version,
            [Random(0x0000, 0xFFFF, 1)] int hardware,
            [Values(0x00, 0x01)] int exrom,
            [Values(0x00, 0x01)] int game,
            [Values(0x00, 0x20)] int nameLength
        )
        {
            // Arrange.
            var extraData = CreateMany<byte>(extraDataLength);
            var reservedData = CreateMany<byte>(6);
            var headerLength = 0x20 + extraDataLength + nameLength;
            var name = CreateAlphanumeric(nameLength);
            var mem = new MemoryStream(new byte[]
                {
                    0x43, 0x36, 0x34, 0x20,
                    0x43, 0x41, 0x52, 0x54,
                    0x52, 0x49, 0x44, 0x47,
                    0x45, 0x20, 0x20, 0x20,
                    unchecked((byte) (headerLength >> 24)), unchecked((byte) (headerLength >> 16)),
                    unchecked((byte) (headerLength >> 8)), unchecked((byte) headerLength),
                    unchecked((byte) (version >> 8)), unchecked((byte) version),
                    unchecked((byte) (hardware >> 8)), unchecked((byte) hardware),
                    unchecked((byte) exrom), unchecked((byte) game)
                }
                .Concat(reservedData)
                .Concat(_stringConverter.ConvertToBytes(name))
                .Concat(extraData)
                .ToArray());

            // Act.
            var output = Subject.Decode(mem);

            // Assert.
            output.ExromPin.Should().Be(exrom != 0, $"{nameof(ICrtInfo.ExromPin)} must be expected");
            output.ExtraData.Should().BeEquivalentTo(extraData, $"{nameof(ICrtInfo.ExtraData)} must be expected");
            output.GamePin.Should().Be(game != 0, $"{nameof(ICrtInfo.GamePin)} must be expected");
            output.Hardware.Should().Be(hardware, $"{nameof(ICrtInfo.Hardware)} must be expected");
            output.Name.Should().Be(name, $"{nameof(ICrtInfo.Name)} must be expected");
            output.ReservedData.Should()
                .BeEquivalentTo(reservedData, $"{nameof(ICrtInfo.ReservedData)} must be expected");
            output.Version.Should().Be(version, $"{nameof(ICrtInfo.Version)} must be expected");
        }

        [Test]
        public void Decode_ThrowsWithIncorrectId()
        {
            // Arrange.
            var cartridgeId = CreateMany<byte>(0x10).ToArray();
            var mem = new MemoryStream(cartridgeId);

            // Act.
            Action act = () => Subject.Decode(mem);

            // Assert.
            act.Should().Throw<CartridgeBuilderException>()
                .WithMessage($"File ID is invalid. Found: {_stringConverter.ConvertToString(cartridgeId)}");
        }

        [Test]
        public void Decode_ThrowsWithTooSmallHeaderSize(
            [Random(0x00, 0x1F, 1)] int headerLength
        )
        {
            // Arrange.
            var mem = new MemoryStream(new byte[]
            {
                0x43, 0x36, 0x34, 0x20,
                0x43, 0x41, 0x52, 0x54,
                0x52, 0x49, 0x44, 0x47,
                0x45, 0x20, 0x20, 0x20,
                unchecked((byte) (headerLength >> 24)), unchecked((byte) (headerLength >> 16)),
                unchecked((byte) (headerLength >> 8)), unchecked((byte) headerLength)
            });

            // Act.
            Action act = () => Subject.Decode(mem);

            // Assert.
            act.Should().Throw<CartridgeBuilderException>()
                .WithMessage($"File header length is invalid. Found: {headerLength:X8}");
        }
    }
}