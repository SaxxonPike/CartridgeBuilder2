using System;
using System.IO;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Crt
{
    [Service]
    public class CrtInfoEncoder : ICrtInfoEncoder
    {
        private readonly IByteSwapper _byteSwapper;
        private readonly IStringConverter _stringConverter;

        public CrtInfoEncoder(
            IStringConverter stringConverter,
            IByteSwapper byteSwapper)
        {
            _stringConverter = stringConverter;
            _byteSwapper = byteSwapper;
        }

        public void Encode(Stream stream, ICrtInfo crtInfo)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (crtInfo == null)
                throw new ArgumentNullException(nameof(crtInfo));

            var writer = new BinaryWriter(stream);

            var fileId = _stringConverter.ConvertToBytes(CrtConstants.FileId);
            writer.Write(fileId);

            var headerLength = _byteSwapper.Swap(0x40 + crtInfo.ExtraData.Count);
            writer.Write(headerLength);

            var version = _byteSwapper.Swap(unchecked((ushort) crtInfo.Version));
            writer.Write(version);

            var hardware = _byteSwapper.Swap(unchecked((ushort) crtInfo.Hardware));
            writer.Write(hardware);

            var exrom = crtInfo.ExromPin ? (byte) 0x01 : (byte) 0x00;
            writer.Write(exrom);

            var game = crtInfo.GamePin ? (byte) 0x01 : (byte) 0x00;
            writer.Write(game);

            var reservedBuffer = new byte[6];
            var reserved = crtInfo.ReservedData.Take(6).AsArray();
            Array.Copy(reserved, reservedBuffer, reserved.Length);
            writer.Write(reservedBuffer);

            var name = _stringConverter.ConvertToBytes(crtInfo.Name, 0x20, 0x00);
            writer.Write(name);

            var extra = crtInfo.ExtraData.AsArray();
            writer.Write(extra);
        }
    }
}