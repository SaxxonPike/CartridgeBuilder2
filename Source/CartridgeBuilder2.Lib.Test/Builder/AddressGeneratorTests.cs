using CartridgeBuilder2.Lib.Builder;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Builder
{
    [TestFixture]
    public class AddressGeneratorTests : BaseUnitTestFixture<AddressGenerator, IAddressGenerator>
    {
        [Test]
        [TestCase(0x0000, 0x8000)]
        [TestCase(0x1000, 0x9000)]
        [TestCase(0x2000, 0xA000)]
        [TestCase(0x3000, 0xB000)]
        [TestCase(0x4000, 0x8000)]
        [TestCase(0x5000, 0x9000)]
        [TestCase(0x6000, 0xA000)]
        [TestCase(0x7000, 0xB000)]
        [TestCase(0x1010, 0x9010)]
        [TestCase(0x2020, 0xA020)]
        [TestCase(0x3030, 0xB030)]
        [TestCase(0x4040, 0x8040)]
        [TestCase(0x5050, 0x9050)]
        [TestCase(0x6060, 0xA060)]
        [TestCase(0x7070, 0xB070)]
        public void GetAddress_ReturnsCorrectMappedAddress(int offset, int expected)
        {
            // Act.
            var output = Subject.GetAddress(offset);
            
            // Assert.
            output.Should().Be(expected);
        }

        [Test]
        [TestCase(0x00000, 0x00)]
        [TestCase(0x01000, 0x00)]
        [TestCase(0x02000, 0x00)]
        [TestCase(0x03000, 0x00)]
        [TestCase(0x04000, 0x01)]
        [TestCase(0x05000, 0x01)]
        [TestCase(0x06000, 0x01)]
        [TestCase(0x07000, 0x01)]
        [TestCase(0x10010, 0x04)]
        [TestCase(0x20020, 0x08)]
        [TestCase(0x30030, 0x0C)]
        [TestCase(0x40040, 0x10)]
        [TestCase(0x50050, 0x14)]
        [TestCase(0x60060, 0x18)]
        [TestCase(0x70070, 0x1C)]
        public void GetBank_ReturnsCorrectMappedBank(int offset, int expected)
        {
            // Act.
            var output = Subject.GetBank(offset);
            
            // Assert.
            output.Should().Be(expected);            
        }
    }
}