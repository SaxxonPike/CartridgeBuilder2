using System.Diagnostics;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Cli.Infrastructure
{
    [Service(typeof(ITimekeeper))]
    public class Timekeeper : Stopwatch, ITimekeeper
    {
    }
}