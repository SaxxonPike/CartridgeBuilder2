using System;
using System.IO;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Crt
{
    [Service(typeof(ICrtChipDecoder))]
    public class CrtChipDecoder : ICrtChipDecoder
    {
        private readonly IByteSwapper _byteSwapper;

        public CrtChipDecoder(IByteSwapper byteSwapper)
        {
            _byteSwapper = byteSwapper;
        }

        public IChip Decode(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            var chipBuffer = new byte[0x10];
            var bytesRead = stream.Read(chipBuffer, 0, 0x10);

            if (bytesRead == 0)
                return null;

            if (bytesRead < 0x10)
                throw new CartridgeBuilderException($"Chip header is less than 16 bytes. Found only {bytesRead} bytes");

            using (var chip = new MemoryStream(chipBuffer))
            {
                var chipReader = new BinaryReader(chip);
                var chipId = chipReader.ReadInt32();

                if (chipId != CrtConstants.ChipId)
                    throw new CartridgeBuilderException($"Chip ID is invalid. Found: {chipId:X8}");

                var chipDataLength = _byteSwapper.Swap(chipReader.ReadInt32());
                if (chipDataLength < 0x10)
                    throw new CartridgeBuilderException($"Chip header length is invalid. Found: {chipDataLength:X8}");

                using (var chipData = new MemoryStream(chipReader.ReadBytes(chipDataLength - 0x10)))
                {
                    var chipDataReader = new BinaryReader(chipData);
                    var chipType = _byteSwapper.Swap(chipDataReader.ReadUInt16());
                    var chipBank = _byteSwapper.Swap(chipDataReader.ReadUInt16());
                    var chipAddress = _byteSwapper.Swap(chipDataReader.ReadUInt16());
                    var chipRomSize = _byteSwapper.Swap(chipDataReader.ReadUInt16());

                    return new Chip
                    {
                        Type = chipType,
                        Bank = chipBank,
                        Address = chipAddress,
                        Rom = chipDataReader.ReadBytes(chipRomSize),
                        ExtraData = chipDataReader.ReadBytes(chipDataLength - chipRomSize - 0x10)
                    };
                }
            }
        }
    }
}