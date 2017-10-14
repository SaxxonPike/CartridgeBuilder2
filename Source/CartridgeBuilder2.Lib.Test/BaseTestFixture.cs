using System;
using System.Diagnostics;
using System.Linq;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Dsl;

namespace CartridgeBuilder2.Lib.Test
{
    /// <summary>
    ///     Contains features available to all tests.
    /// </summary>
    public abstract class BaseTestFixture
    {
        private const string AlphaNumericCharacters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private readonly Lazy<Fixture> _fixture = new Lazy<Fixture>(() =>
        {
            var fixture = new Fixture();
            new SupportMutableValueTypesCustomization().Customize(fixture);
            return fixture;
        });

        private Stopwatch _stopwatch;

        [SetUp]
        public void __Setup()
        {
            _stopwatch = new Stopwatch();
            _stopwatch.Start();
        }

        [TearDown]
        public void __Teardown()
        {
            _stopwatch.Stop();
            TestContext.Out.WriteLine(
                $"{TestContext.CurrentContext.Test.FullName}: {_stopwatch.ElapsedMilliseconds}ms");
        }

        /// <summary>
        ///     Gets an AutoFixture builder which can be used to customize a created object.
        /// </summary>
        protected ICustomizationComposer<T> Build<T>()
        {
            return _fixture.Value.Build<T>();
        }

        /// <summary>
        ///     Creates an object of the specified type with randomized properties.
        /// </summary>
        protected T Create<T>()
        {
            return _fixture.Value.Create<T>();
        }

        /// <summary>
        ///     Creates many objects of the specified type all with randomized properties.
        /// </summary>
        protected T[] CreateMany<T>()
        {
            return _fixture.Value.CreateMany<T>().ToArray();
        }

        /// <summary>
        ///     Creates a specified number of objects of the specified type all with randomized properties.
        /// </summary>
        protected T[] CreateMany<T>(int count)
        {
            return _fixture.Value.CreateMany<T>(count).ToArray();
        }

        /// <summary>
        ///     Create an alphanumeric string.
        /// </summary>
        protected string CreateAlphanumeric(int length)
        {
            return new string(CreateMany<int>(length)
                .Select(i => AlphaNumericCharacters[i % AlphaNumericCharacters.Length])
                .ToArray());
        }
    }
}