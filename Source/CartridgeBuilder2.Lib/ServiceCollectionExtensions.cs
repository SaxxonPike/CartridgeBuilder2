using System.Linq;
using System.Reflection;
using CartridgeBuilder2.Lib.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace CartridgeBuilder2.Lib
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCartridgeBuilderLib(this IServiceCollection serviceCollection)
        {
            foreach (var (type, iface) in typeof(ServiceCollectionExtensions).Assembly.DefinedTypes
                .SelectMany(t => t.GetCustomAttributes<ServiceAttribute>()
                    .Select(a => (ServiceType: t, ServiceInterface: a.Interface))))
                serviceCollection.AddSingleton(iface, type);
        }
    }
}