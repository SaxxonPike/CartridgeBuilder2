using System.Collections.Generic;
using System.IO;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Lib.Crt
{
    [Service(typeof(ICrtFileDecoder))]
    public class CrtFileDecoder : ICrtFileDecoder
    {
        private readonly ICrtChipDecoder _crtChipDecoder;
        private readonly ICrtInfoDecoder _crtInfoDecoder;

        public CrtFileDecoder(
            ICrtInfoDecoder crtInfoDecoder,
            ICrtChipDecoder crtChipDecoder)
        {
            _crtInfoDecoder = crtInfoDecoder;
            _crtChipDecoder = crtChipDecoder;
        }

        public ICrtFile Decode(Stream stream)
        {
            var bufferedStream = new BufferedStream(stream);
            return new CrtFile
            {
                Info = _crtInfoDecoder.Decode(bufferedStream),
                Chips = GetChips(bufferedStream).ToList()
            };
        }

        private IEnumerable<IChip> GetChips(Stream stream)
        {
            while (true)
            {
                var chip = _crtChipDecoder.Decode(stream);
                if (chip == null)
                    yield break;
                yield return chip;
            }
        }
    }
}