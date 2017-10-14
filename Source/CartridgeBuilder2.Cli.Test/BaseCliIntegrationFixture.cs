using System.Collections.Generic;
using System.IO;
using System.Linq;
using Autofac;
using Autofac.Core;
using CartridgeBuilder2.Cli.Configuration;
using CartridgeBuilder2.Cli.Infrastructure;
using CartridgeBuilder2.Lib.Test;
using Newtonsoft.Json;

namespace CartridgeBuilder2.Cli.Test
{
    public class BaseCliIntegrationFixture : BaseIntegrationFixture<App>
    {
        protected override IEnumerable<IModule> ContainerModules { get; } = new Module[]
        {
            new AppAutofacModule(),
            new TestAutofacModule()
        };

        protected void WriteConfig(string path, BuilderConfig config)
        {
            using (var writer = new StreamWriter(FileSystem.OpenWrite(path)))
            {
                var json = JsonConvert.SerializeObject(config);
                writer.Write(json);
                writer.Flush();
            }
        }

        protected void WriteFile(string path, IEnumerable<byte> data)
        {
            FileSystem.WriteAllBytes(path, data as byte[] ?? data.ToArray());
        }

        protected IFileSystem FileSystem => Resolve<IFileSystem>();
    }
}