using System.IO;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface ICrtChipEncoder
    {
        void Encode(Stream stream, IChip chip);
    }
}