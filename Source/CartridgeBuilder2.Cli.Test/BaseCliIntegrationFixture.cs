using System.Collections.Generic;
using System.IO;
using System.Linq;
using CartridgeBuilder2.Cli.Configuration;
using CartridgeBuilder2.Cli.Infrastructure;
using CartridgeBuilder2.Lib.Test;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace CartridgeBuilder2.Cli.Test
{
    public class BaseCliIntegrationFixture : BaseIntegrationFixture<IApp>
    {
        protected void WriteConfig(string path, BuilderConfig config)
        {
            using var writer = new StreamWriter(FileSystem.OpenWrite(path));
            var json = JsonConvert.SerializeObject(config);
            writer.Write(json);
            writer.Flush();
        }

        protected void WriteFile(string path, IEnumerable<byte> data)
        {
            FileSystem.WriteAllBytes(path, data as byte[] ?? data.ToArray());
        }

        protected IFileSystem FileSystem => Resolve<IFileSystem>();

        protected override void BeforeBuildContainer(IServiceCollection serviceCollection)
        {
            serviceCollection.AddCartridgeBuilderCli();
            serviceCollection.AddSingleton(typeof(IFileSystem), new FakeFileSystem());
            // serviceCollection.AddSingleton(typeof(IApp), typeof(App));
            base.BeforeBuildContainer(serviceCollection);
        }
    }
}