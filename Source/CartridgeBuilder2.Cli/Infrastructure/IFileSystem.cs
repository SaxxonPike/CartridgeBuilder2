using System.Collections.Generic;
using System.IO;

namespace CartridgeBuilder2.Cli.Infrastructure
{
    public interface IFileSystem
    {
        IEnumerable<string> Expand(string path);
        string GetName(string path);
        Stream OpenRead(string path);
        Stream OpenWrite(string path);
        byte[] ReadAllBytes(string path);
        void WriteAllBytes(string path, byte[] data);
    }
}