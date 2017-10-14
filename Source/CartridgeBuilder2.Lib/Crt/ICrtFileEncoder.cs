using System.IO;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface ICrtFileEncoder
    {
        void Encode(Stream stream, ICrtFile crtFile);
    }
}