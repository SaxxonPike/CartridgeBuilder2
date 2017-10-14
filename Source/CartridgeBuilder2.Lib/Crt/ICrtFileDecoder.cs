using System.IO;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface ICrtFileDecoder
    {
        ICrtFile Decode(Stream stream);
    }
}