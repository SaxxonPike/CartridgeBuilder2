namespace CartridgeBuilder2.Lib.Infrastructure
{
    [Service]
    public class ByteSwapper : IByteSwapper
    {
        public short Swap(short value)
        {
            return unchecked((short) ((value << 8) | ((value >> 8) & 0xFF)));
        }

        public ushort Swap(ushort value)
        {
            return unchecked((ushort) ((value << 8) | (value >> 8)));
        }

        public int Swap(int value)
        {
            return (value << 24) | ((value << 8) & 0xFF0000) | ((value >> 8) & 0xFF00) |
                   ((value >> 24) & 0xFF);
        }

        public uint Swap(uint value)
        {
            return (value << 24) | ((value << 8) & 0xFF0000) | ((value >> 8) & 0xFF00) |
                   (value >> 24);
        }
    }
}