using System.IO;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Crt
{
    [Service]
    public class CrtFileEncoder : ICrtFileEncoder
    {
        private readonly ICrtChipEncoder _crtChipEncoder;
        private readonly ICrtInfoEncoder _crtInfoEncoder;

        public CrtFileEncoder(ICrtInfoEncoder crtInfoEncoder, ICrtChipEncoder crtChipEncoder)
        {
            _crtInfoEncoder = crtInfoEncoder;
            _crtChipEncoder = crtChipEncoder;
        }

        public void Encode(Stream stream, ICrtFile crtFile)
        {
            var bufferedStream = new BufferedStream(stream);

            _crtInfoEncoder.Encode(bufferedStream, crtFile.Info);

            foreach (var chip in crtFile.Chips)
                _crtChipEncoder.Encode(bufferedStream, chip);

            bufferedStream.Flush();
        }
    }
}