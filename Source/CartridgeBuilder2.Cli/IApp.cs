using System.Collections.Generic;

namespace CartridgeBuilder2.Cli
{
    public interface IApp
    {
        void Run(IList<string> args);
    }
}