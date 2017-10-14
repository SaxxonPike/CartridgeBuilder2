using CartridgeBuilder2.Lib.Builder;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Builder
{
    public class UsageTypeTests : BaseUnitTestFixture
    {
        [Test]
        public void UsageType_DefaultIsUnused()
        {
            // Assert.
            default(UsageType).Should().Be(UsageType.Unused);
        }
    }
}