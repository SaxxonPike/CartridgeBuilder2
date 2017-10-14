using System;
using Autofac;
using CartridgeBuilder2.Cli.Infrastructure;
using CartridgeBuilder2.Lib.Infrastructure;

namespace CartridgeBuilder2.Cli
{
    public class InfrastructureAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterInstance(Console.Out)
                .AsImplementedInterfaces()
                .AsSelf()
                .ExternallyOwned()
                .SingleInstance();

            builder.RegisterType<TextWriterLogger>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<FileSystem>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}