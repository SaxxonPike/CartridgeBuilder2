namespace CartridgeBuilder2.Lib.Infrastructure
{
    public interface IByteSwapper
    {
        short Swap(short value);
        ushort Swap(ushort value);
        int Swap(int value);
        uint Swap(uint value);
    }
}