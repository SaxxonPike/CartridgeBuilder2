using System.IO;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface ICrtInfoDecoder
    {
        ICrtInfo Decode(Stream stream);
    }
}