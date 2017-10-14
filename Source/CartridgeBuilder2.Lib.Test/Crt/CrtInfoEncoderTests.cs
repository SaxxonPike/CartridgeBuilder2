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
    public class CrtInfoEncoderTests : BaseUnitTestFixture<CrtInfoEncoder, ICrtInfoEncoder>
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
        public void Encode_ThrowsIfInfoIsNull()
        {
            // Arrange.
            var info = new CrtInfo();

            // Act.
            Action act = () => Subject.Encode(null, info);

            // Assert.
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Encode_ThrowsIfStreamIsNull()
        {
            // Arrange.
            var mem = new MemoryStream();

            // Act.
            Action act = () => Subject.Encode(mem, null);

            // Assert.
            act.ShouldThrow<ArgumentNullException>();
        }

        [Test]
        public void Encode_WritesCorrectBytes(
            [Random(0x0000, 0xFFFF, 1)] int version,
            [Random(0x0000, 0xFFFF, 1)] int hardware,
            [Values(false, true)] bool exrom,
            [Values(false, true)] bool game,
            [Values(0x0000, 0x0001, 0x0010)] int extraDataSize
        )
        {
            // Arrange.
            var expectedExtraData = CreateMany<byte>(extraDataSize);
            var expectedReservedData = CreateMany<byte>(6);
            var name = CreateAlphanumeric(0x20);
            var mem = new MemoryStream();
            var headerLength = extraDataSize + 0x40;
            var info = new CrtInfo
            {
                ExromPin = exrom,
                ExtraData = expectedExtraData,
                GamePin = game,
                Hardware = hardware,
                Name = name,
                ReservedData = expectedReservedData,
                Version = version
            };

            // Act.
            Subject.Encode(mem, info);
            mem.Flush();

            // Assert.
            mem.ToArray().Should().BeEquivalentTo(new byte[]
                {
                    0x43, 0x36, 0x34, 0x20,
                    0x43, 0x41, 0x52, 0x54,
                    0x52, 0x49, 0x44, 0x47,
                    0x45, 0x20, 0x20, 0x20,
                    unchecked((byte) (headerLength >> 24)), unchecked((byte) (headerLength >> 16)),
                    unchecked((byte) (headerLength >> 8)), unchecked((byte) headerLength),
                    unchecked((byte) (version >> 8)), unchecked((byte) version),
                    unchecked((byte) (hardware >> 8)), unchecked((byte) hardware),
                    exrom ? (byte) 1 : (byte) 0, game ? (byte) 1 : (byte) 0
                }
                .Concat(expectedReservedData)
                .Concat(_stringConverter.ConvertToBytes(name))
                .Concat(expectedExtraData).ToArray());
        }
    }
}