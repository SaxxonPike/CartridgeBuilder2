using CartridgeBuilder2.Lib.Builder;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Builder
{
    [TestFixture]
    public class CompressionMethodTests : BaseUnitTestFixture
    {
        [Test]
        public void CompressionMethod_DefaultIsNone()
        {
            default(CompressionMethod).Should().Be(CompressionMethod.None);
        }
    }
}