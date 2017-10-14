using System;
using System.Collections.Generic;
using System.IO;
using Autofac;
using Autofac.Core;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test
{
    /// <inheritdoc />
    /// <summary>
    ///     Base test fixture for all integration tests that use a simple container.
    /// </summary>
    public abstract class BaseIntegrationFixture<TSubject> : BaseTestFixture
    {
        private readonly Lazy<IContainer> _container;
        private readonly Lazy<TSubject> _subject;

        protected BaseIntegrationFixture()
        {
            _container = new Lazy<IContainer>(BuildContainer);
            _subject = new Lazy<TSubject>(Resolve<TSubject>);
        }

        private IContainer Container => _container.Value;

        protected abstract IEnumerable<IModule> ContainerModules { get; }

        /// <summary>
        ///     Gets the test subject from the container.
        /// </summary>
        protected TSubject Subject => _subject.Value;

        private IContainer BuildContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterInstance(TestContext.Out).As<TextWriter>().SingleInstance();

            foreach (var module in ContainerModules)
                builder.RegisterModule(module);

            return builder.Build();
        }

        /// <summary>
        ///     Gets an object from the container of the specified type.
        /// </summary>
        protected TObject Resolve<TObject>()
        {
            return Container.Resolve<TObject>();
        }
    }
}