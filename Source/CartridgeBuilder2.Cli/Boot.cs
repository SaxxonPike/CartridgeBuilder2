using System;
using System.Diagnostics;
using System.IO;
using CartridgeBuilder2.Lib;
using CartridgeBuilder2.Lib.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CartridgeBuilder2.Cli
{
    internal static class Boot
    {
        private static void Main(string[] args)
        {
            var builder = new ServiceCollection();
            builder.AddCartridgeBuilderLib();
            builder.AddCartridgeBuilderCli();
            builder.AddSingleton(typeof(TextWriter), Console.Out);
            var container = builder.BuildServiceProvider();
            
            var app = container.GetService<IApp>();
            var logger = container.GetService<ILogger>();

            if (Debugger.IsAttached)
                app.Run(args);
            else
                try
                {
                    app.Run(args);
                }
                catch (Exception e)
                {
                    logger.Error($"An exception of type {e.GetType().Name} was thrown:");
                    logger.Error(e.ToString());
                }
        }
    }
}