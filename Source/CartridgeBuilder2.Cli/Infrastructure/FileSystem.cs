using System.Collections.Generic;
using System.IO;
using System.Linq;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Cli.Infrastructure
{
    public class FileSystem : IFileSystem
    {
        /// <inheritdoc />
        public IEnumerable<string> Expand(string path)
        {
            if (Directory.Exists(path))
                return Directory.GetFiles(path);
            else if (File.Exists(path))
                return new[] {path};
            return Enumerable.Empty<string>();
        }

        /// <inheritdoc />
        public string GetName(string path)
        {
            return Path.GetFileName(path);
        }

        /// <inheritdoc />
        public Stream OpenRead(string path)
        {
            return File.OpenRead(path);
        }

        /// <inheritdoc />
        public Stream OpenWrite(string path)
        {
            return File.OpenWrite(path);
        }

        /// <inheritdoc />
        public byte[] ReadAllBytes(string path)
        {
            return File.ReadAllBytes(path);
        }

        /// <inheritdoc />
        public void WriteAllBytes(string path, IEnumerable<byte> data)
        {
            File.WriteAllBytes(path, data.AsArray());
        }
    }
}