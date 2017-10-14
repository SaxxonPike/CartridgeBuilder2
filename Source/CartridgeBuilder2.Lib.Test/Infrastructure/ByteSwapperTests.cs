using CartridgeBuilder2.Lib.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Infrastructure
{
    [TestFixture]
    public class ByteSwapperTests : BaseUnitTestFixture<ByteSwapper, IByteSwapper>
    {
        [Test]
        [TestCase(0x00000000, 0x00000000)]
        [TestCase(0x00000001, 0x01000000)]
        [TestCase(0x00000008, 0x08000000)]
        [TestCase(0x00000010, 0x10000000)]
        [TestCase(0x00000080, 0x80000000)]
        [TestCase(0x00000100, 0x00010000)]
        [TestCase(0x00000800, 0x00080000)]
        [TestCase(0x00001000, 0x00100000)]
        [TestCase(0x00008000, 0x00800000)]
        [TestCase(0x00010000, 0x00000100)]
        [TestCase(0x00080000, 0x00000800)]
        [TestCase(0x00100000, 0x00001000)]
        [TestCase(0x00800000, 0x00008000)]
        [TestCase(0x01000000, 0x00000001)]
        [TestCase(0x08000000, 0x00000008)]
        [TestCase(0x10000000, 0x00000010)]
        [TestCase(0x80000000, 0x00000080)]
        public void Swap_Int_SwapsCorrectly(long source, long expected)
        {
            // Arrange.
            var value = unchecked((int) source);

            // Act.
            var output = Subject.Swap(value);

            // Assert.
            output.Should().Be(unchecked((int) expected));
        }

        [Test]
        [TestCase(0x0000, 0x0000)]
        [TestCase(0x0001, 0x0100)]
        [TestCase(0x0008, 0x0800)]
        [TestCase(0x0010, 0x1000)]
        [TestCase(0x0080, 0x8000)]
        [TestCase(0x0100, 0x0001)]
        [TestCase(0x0800, 0x0008)]
        [TestCase(0x1000, 0x0010)]
        [TestCase(0x8000, 0x0080)]
        public void Swap_Short_SwapsCorrectly(long source, long expected)
        {
            // Arrange.
            var value = unchecked((short) source);

            // Act.
            var output = Subject.Swap(value);

            // Assert.
            output.Should().Be(unchecked((short) expected));
        }

        [Test]
        [TestCase(0x00000000, 0x00000000)]
        [TestCase(0x00000001, 0x01000000)]
        [TestCase(0x00000008, 0x08000000)]
        [TestCase(0x00000010, 0x10000000)]
        [TestCase(0x00000080, 0x80000000)]
        [TestCase(0x00000100, 0x00010000)]
        [TestCase(0x00000800, 0x00080000)]
        [TestCase(0x00001000, 0x00100000)]
        [TestCase(0x00008000, 0x00800000)]
        [TestCase(0x00010000, 0x00000100)]
        [TestCase(0x00080000, 0x00000800)]
        [TestCase(0x00100000, 0x00001000)]
        [TestCase(0x00800000, 0x00008000)]
        [TestCase(0x01000000, 0x00000001)]
        [TestCase(0x08000000, 0x00000008)]
        [TestCase(0x10000000, 0x00000010)]
        [TestCase(0x80000000, 0x00000080)]
        public void Swap_Uint_SwapsCorrectly(long source, long expected)
        {
            // Arrange.
            var value = unchecked((uint) source);

            // Act.
            var output = Subject.Swap(value);

            // Assert.
            output.Should().Be(unchecked((uint) expected));
        }

        [Test]
        [TestCase(0x0000, 0x0000)]
        [TestCase(0x0001, 0x0100)]
        [TestCase(0x0008, 0x0800)]
        [TestCase(0x0010, 0x1000)]
        [TestCase(0x0080, 0x8000)]
        [TestCase(0x0100, 0x0001)]
        [TestCase(0x0800, 0x0008)]
        [TestCase(0x1000, 0x0010)]
        [TestCase(0x8000, 0x0080)]
        public void Swap_Ushort_SwapsCorrectly(long source, long expected)
        {
            // Arrange.
            var value = unchecked((ushort) source);

            // Act.
            var output = Subject.Swap(value);

            // Assert.
            output.Should().Be(unchecked((ushort) expected));
        }
    }
}