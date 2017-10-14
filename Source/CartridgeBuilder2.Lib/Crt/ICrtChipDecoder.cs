using System.IO;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface ICrtChipDecoder
    {
        IChip Decode(Stream stream);
    }
}