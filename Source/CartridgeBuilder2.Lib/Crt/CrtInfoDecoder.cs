using System.IO;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Crt
{
    [Service(typeof(ICrtInfoDecoder))]
    public class CrtInfoDecoder : ICrtInfoDecoder
    {
        private readonly IByteSwapper _byteSwapper;
        private readonly IStringConverter _stringConverter;

        public CrtInfoDecoder(IByteSwapper byteSwapper, IStringConverter stringConverter)
        {
            _byteSwapper = byteSwapper;
            _stringConverter = stringConverter;
        }

        public ICrtInfo Decode(Stream stream)
        {
            var reader = new BinaryReader(stream);

            var cartridgeId = _stringConverter.ConvertToString(reader.ReadBytes(0x10));
            if (cartridgeId != CrtConstants.FileId)
                throw new CartridgeBuilderException($"File ID is invalid. Found: {cartridgeId}");

            var headerLength = _byteSwapper.Swap(reader.ReadInt32());
            if (headerLength < 0x20)
                throw new CartridgeBuilderException($"File header length is invalid. Found: {headerLength:X8}");

            using (var header = new MemoryStream(reader.ReadBytes(headerLength - 0x14)))
            {
                var headerReader = new BinaryReader(header);
                var isLongForm = headerLength >= 0x40;

                return new CrtInfo
                {
                    Version = _byteSwapper.Swap(headerReader.ReadUInt16()),
                    Hardware = _byteSwapper.Swap(headerReader.ReadUInt16()),
                    ExromPin = headerReader.ReadByte() != 0,
                    GamePin = headerReader.ReadByte() != 0,
                    ReservedData = headerReader.ReadBytes(6),
                    Name = isLongForm
                        ? new string(headerReader.ReadChars(0x20))
                        : string.Empty,
                    ExtraData = headerReader
                        .ReadBytes(headerLength - (isLongForm ? 0x40 : 0x20))
                };
            }
        }
    }
}