using System.Collections.Generic;
using System.IO;
using System.Linq;
using CartridgeBuilder2.Cli.Infrastructure;

namespace CartridgeBuilder2.Cli.Test
{
    public class FakeFileSystem : IFileSystem
    {
        private readonly IDictionary<string, MemoryStream> _streams = new Dictionary<string, MemoryStream>();

        public IEnumerable<string> Expand(string path)
        {
            yield return path;
        }

        public string GetName(string path)
        {
            return Path.GetFileName(path);
        }

        public Stream OpenRead(string path)
        {
            return new MemoryStream(_streams[path.ToLowerInvariant()].ToArray(), false);
        }

        public Stream OpenWrite(string path)
        {
            var mem = new MemoryStream();
            _streams[path.ToLowerInvariant()] = mem;
            return mem;
        }

        public byte[] ReadAllBytes(string path)
        {
            return _streams[path.ToLowerInvariant()].ToArray();
        }

        public void WriteAllBytes(string path, byte[] data)
        {
            _streams[path.ToLowerInvariant()] = new MemoryStream(data);
        }
    }
}