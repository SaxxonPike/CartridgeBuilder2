using System.IO;

namespace CartridgeBuilder2.Lib.Crt
{
    public interface ICrtInfoEncoder
    {
        void Encode(Stream stream, ICrtInfo crtInfo);
    }
}