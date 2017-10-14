using CartridgeBuilder2.Lib.Builder;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Builder
{
    [TestFixture]
    public class WrapStrategyTests : BaseTestFixture
    {
        [Test]
        public void WrapStrategy_DefaultIsBothBanks()
        {
            // Assert.
            default(WrapStrategy).Should().Be(WrapStrategy.Both);
        }
    }
}