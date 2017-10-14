using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Autofac;
using CartridgeBuilder2.Lib.Infrastructure;
using Module = Autofac.Module;

namespace CartridgeBuilder2.Cli
{
    /// <inheritdoc />
    public class AppAutofacModule : Module
    {
        /// <summary>
        ///     A single type from each assembly that needs to be auto-loaded.
        /// </summary>
        private static readonly IEnumerable<Type> IocTypes = new[]
        {
            typeof(App),
            typeof(ServiceAttribute)
        };

        /// <inheritdoc />
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);
            foreach (var assembly in IocTypes.Select(t => t.GetTypeInfo().Assembly).Distinct())
                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.GetTypeInfo().CustomAttributes.All(a => a.AttributeType == typeof(ServiceAttribute)))
                    .AsSelf()
                    .AsImplementedInterfaces()
                    .SingleInstance();
        }
    }
}