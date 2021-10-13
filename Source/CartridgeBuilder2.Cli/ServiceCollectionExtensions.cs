using System.Linq;
using System.Reflection;
using CartridgeBuilder2.Lib.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CartridgeBuilder2.Cli
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCartridgeBuilderCli(this IServiceCollection serviceCollection)
        {
            foreach (var (type, iface) in typeof(ServiceCollectionExtensions).Assembly.DefinedTypes
                .SelectMany(t => t.GetCustomAttributes<ServiceAttribute>()
                    .Select(a => (ServiceType: t, ServiceInterface: a.Interface))))
                serviceCollection.AddSingleton(iface, type);
        }
    }
}