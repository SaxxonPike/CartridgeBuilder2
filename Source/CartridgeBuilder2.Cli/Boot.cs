using System;
using System.Diagnostics;
using Autofac;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Cli
{
    internal static class Boot
    {
        private static void Main(string[] args)
        {
            var container = BuildContainer();
            var app = container.Resolve<App>();
            var logger = container.Resolve<ILogger>();

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

        private static IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterModule<InfrastructureAutofacModule>();
            builder.RegisterModule<AppAutofacModule>();
            return builder.Build();
        }
    }
}