using Autofac;
using CartridgeBuilder2.Lib.Infrastructure;
using NUnit.Framework;

namespace CartridgeBuilder2.Cli.Test
{
    public class TestAutofacModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.RegisterInstance(TestContext.Out)
                .AsImplementedInterfaces()
                .AsSelf()
                .ExternallyOwned()
                .SingleInstance();

            builder.RegisterType<TextWriterLogger>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();

            builder.RegisterType<FakeFileSystem>()
                .AsImplementedInterfaces()
                .AsSelf()
                .SingleInstance();
        }
    }
}