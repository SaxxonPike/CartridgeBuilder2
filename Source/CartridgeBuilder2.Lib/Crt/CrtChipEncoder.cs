using System;
using System.IO;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Crt
{
    [Service(typeof(ICrtChipEncoder))]
    public class CrtChipEncoder : ICrtChipEncoder
    {
        private readonly IByteSwapper _byteSwapper;

        public CrtChipEncoder(IByteSwapper byteSwapper)
        {
            _byteSwapper = byteSwapper;
        }

        public void Encode(Stream stream, IChip chip)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (chip == null)
                throw new ArgumentNullException(nameof(chip));

            var writer = new BinaryWriter(stream);

            var chipId = CrtConstants.ChipId;
            writer.Write(chipId);

            var length = _byteSwapper.Swap((chip.Rom?.Length ?? 0) + 0x10 + (chip.ExtraData?.Length ?? 0));
            writer.Write(length);

            var chipType = _byteSwapper.Swap(unchecked((ushort) chip.Type));
            writer.Write(chipType);

            var chipBank = _byteSwapper.Swap(unchecked((ushort) chip.Bank));
            writer.Write(chipBank);

            var chipAddress = _byteSwapper.Swap(unchecked((ushort) chip.Address));
            writer.Write(chipAddress);

            var chipRomLength = _byteSwapper.Swap(unchecked((ushort) (chip.Rom?.Length ?? 0)));
            writer.Write(chipRomLength);

            if (chip.ExtraData != null)
            {
                var chipExtraData = chip.ExtraData.AsArray();
                writer.Write(chipExtraData);
            }

            if (chip.Rom != null)
            {
                var chipRom = chip.Rom.AsArray();
                writer.Write(chipRom);
            }
        }
    }
}