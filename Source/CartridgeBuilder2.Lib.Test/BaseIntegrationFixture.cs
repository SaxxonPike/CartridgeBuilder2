using System;
using System.Collections.Generic;
using System.IO;
using CartridgeBuilder2.Lib.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test
{
    /// <inheritdoc />
    /// <summary>
    ///     Base test fixture for all integration tests that use a simple container.
    /// </summary>
    public abstract class BaseIntegrationFixture<TSubject> : BaseTestFixture
    {
        private readonly Lazy<IServiceProvider> _container;
        private readonly Lazy<TSubject> _subject;

        protected BaseIntegrationFixture()
        {
            _container = new Lazy<IServiceProvider>(BuildContainer);
            _subject = new Lazy<TSubject>(Resolve<TSubject>);
        }

        private IServiceProvider Container => _container.Value;

        /// <summary>
        ///     Gets the test subject from the container.
        /// </summary>
        protected TSubject Subject => _subject.Value;

        private IServiceProvider BuildContainer()
        {
            var builder = new ServiceCollection();
            builder.AddCartridgeBuilderLib();
            builder.AddSingleton(typeof(TextWriter), TestContext.Out);
            builder.AddSingleton(typeof(ILogger), new TextWriterLogger(TestContext.Out));
            BeforeBuildContainer(builder);
            return builder.BuildServiceProvider();
        }

        protected virtual void BeforeBuildContainer(IServiceCollection serviceCollection)
        {
        }

        /// <summary>
        ///     Gets an object from the container of the specified type.
        /// </summary>
        protected TObject Resolve<TObject>()
        {
            return Container.GetService<TObject>();
        }
    }
}