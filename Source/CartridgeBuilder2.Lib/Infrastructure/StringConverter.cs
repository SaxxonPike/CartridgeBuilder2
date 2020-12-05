using System;
using System.Linq;
using System.Text;

namespace CartridgeBuilder2.Lib.Infrastructure
{
    [Service]
    public class StringConverter : IStringConverter
    {
        // Using ISO-8859-1 encoding ensures all 256 bytes are mapped to equivalent to unicode values
        private static readonly Encoding Encoding = Encoding.GetEncoding("ISO-8859-1");

        public byte[] ConvertToBytes(string value)
        {
            return Encoding.GetBytes(value);
        }

        public byte[] ConvertToBytes(string value, int length)
        {
            var buffer = Enumerable.Repeat((byte) 0x00, length).ToArray();
            var bytes = Encoding.GetBytes(value);
            Array.Copy(bytes, buffer, Math.Min(bytes.Length, buffer.Length));
            return buffer;
        }

        public byte[] ConvertToBytes(string value, int length, byte padding)
        {
            var buffer = Enumerable.Repeat(padding, length).ToArray();
            var bytes = Encoding.GetBytes(value ?? string.Empty);
            Array.Copy(bytes, buffer, Math.Min(bytes.Length, buffer.Length));
            return buffer;
        }

        public string ConvertToString(byte[] value)
        {
            return Encoding.GetString(value);
        }

        public string ConvertToString(byte[] value, byte padding)
        {
            var length = 0;
            for (var i = value.Length - 1; i >= 0; i--)
            {
                if (value[i] == padding)
                    continue;

                length = i + 1;
                break;
            }
            return Encoding.GetString(value, 0, length);
        }
    }
}