using System;

namespace CartridgeBuilder2.Cli.Infrastructure
{
    public interface ITimekeeper
    {
        void Start();
        void Stop();
        void Reset();
        TimeSpan Elapsed { get; }
    }
}