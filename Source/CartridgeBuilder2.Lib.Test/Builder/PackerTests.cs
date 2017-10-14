using System;
using System.Linq;
using CartridgeBuilder2.Lib.Builder;
using CartridgeBuilder2.Lib.Infrastructure;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Builder
{
    [TestFixture]
    public class PackerTests : BaseUnitTestFixture<Packer, IPacker>
    {
        [SetUp]
        public void SetupMocks()
        {
            Mocker.Implement<IIndexGeneratorFactory>(new IndexGeneratorFactory());
        }

        [Test]
        public void Fit_WithEmptyRom_FitsData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);

            // Act.
            var output = Subject.Fit(romSpace, 0x1000);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = 0x1000,
                Offset = 0x0000,
                WrapStrategy = WrapStrategy.Both
            });
        }

        [Test]
        public void Fit_WithReservedStart_FitsData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            for (var i = 0x0000; i <= 0x3FFF; i++)
                romSpace.Usage[i] = UsageType.Used;

            // Act.
            var output = Subject.Fit(romSpace, 0x1000);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = 0x1000,
                Offset = 0x4000,
                WrapStrategy = WrapStrategy.Both
            });
        }

        [Test]
        public void Fit_WithReservedEnd_FitsData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            for (var i = 0x4000; i <= 0x7FFF; i++)
                romSpace.Usage[i] = UsageType.Used;

            // Act.
            var output = Subject.Fit(romSpace, 0x1000);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = 0x1000,
                Offset = 0x0000,
                WrapStrategy = WrapStrategy.Both
            });
        }

        [Test]
        public void Fit_WithReservedLow_FitsData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            for (var i = 0x0000; i <= 0x1FFF; i++)
            {
                romSpace.Usage[i] = UsageType.Used;
                romSpace.Usage[i + 0x4000] = UsageType.Used;
            }

            // Act.
            var output = Subject.Fit(romSpace, 0x4000);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = 0x4000,
                Offset = 0x2000,
                WrapStrategy = WrapStrategy.High
            });
        }

        [Test]
        public void Fit_WithReservedHigh_FitsData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            for (var i = 0x2000; i <= 0x3FFF; i++)
            {
                romSpace.Usage[i] = UsageType.Used;
                romSpace.Usage[i + 0x4000] = UsageType.Used;
            }

            // Act.
            var output = Subject.Fit(romSpace, 0x4000);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = 0x4000,
                Offset = 0x0000,
                WrapStrategy = WrapStrategy.Low
            });
        }

        [Test]
        public void Fit_WithoutAvailableSpace_ReturnsNull()
        {
            // Arrange.
            var romSpace = new RomSpace(0x4000);
            for (var i = 0x2000; i <= 0x3FFF; i++)
            {
                romSpace.Usage[i] = UsageType.Used;
            }

            // Act.
            var output = Subject.Fit(romSpace, 0x4000);

            // Assert.
            output.Should().BeNull();
        }

        [Test]
        public void Reserve_WithLowStrategy_ReservesData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            var allocation = new Allocation
            {
                Length = 0x4000,
                Offset = 0x0000,
                WrapStrategy = WrapStrategy.Low
            };

            // Act.
            var output = Subject.Reserve(romSpace, allocation);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = allocation.Length,
                Offset = 0x0000,
                WrapStrategy = allocation.WrapStrategy
            });
            var expected = Enumerable.Repeat(UsageType.Reserved, 0x2000)
                .Concat(Enumerable.Repeat(UsageType.Unused, 0x2000))
                .Concat(Enumerable.Repeat(UsageType.Reserved, 0x2000))
                .Concat(Enumerable.Repeat(UsageType.Unused, 0x2000));
            romSpace.Usage.SequenceEqual(expected).Should().BeTrue();
        }

        [Test]
        public void Reserve_WithHighStrategy_ReservesData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            var allocation = new Allocation
            {
                Length = 0x4000,
                Offset = 0x2000,
                WrapStrategy = WrapStrategy.High
            };

            // Act.
            var output = Subject.Reserve(romSpace, allocation);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = allocation.Length,
                Offset = 0x2000,
                WrapStrategy = allocation.WrapStrategy
            });
            var expected = Enumerable.Repeat(UsageType.Unused, 0x2000)
                .Concat(Enumerable.Repeat(UsageType.Reserved, 0x2000))
                .Concat(Enumerable.Repeat(UsageType.Unused, 0x2000))
                .Concat(Enumerable.Repeat(UsageType.Reserved, 0x2000));
            romSpace.Usage.SequenceEqual(expected).Should().BeTrue();
        }

        [Test]
        public void Reserve_WithBothStrategy_ReservesData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            var allocation = new Allocation
            {
                Length = 0x4000,
                Offset = 0x2000,
                WrapStrategy = WrapStrategy.Both
            };

            // Act.
            var output = Subject.Reserve(romSpace, allocation);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = allocation.Length,
                Offset = 0x2000,
                WrapStrategy = allocation.WrapStrategy
            });
            var expected = Enumerable.Repeat(UsageType.Unused, 0x2000)
                .Concat(Enumerable.Repeat(UsageType.Reserved, 0x4000))
                .Concat(Enumerable.Repeat(UsageType.Unused, 0x2000));
            romSpace.Usage.SequenceEqual(expected).Should().BeTrue();
        }

        [Test]
        public void Reserve_WithoutAvailableSpace_Throws()
        {
            // Arrange.
            var romSpace = new RomSpace(0x4000);

            var allocation = new Allocation
            {
                Length = 0x8000,
                Offset = 0x0000,
                WrapStrategy = WrapStrategy.Both
            };

            // Act.
            Action act = () => Subject.Reserve(romSpace, allocation);

            // Assert.
            act.ShouldThrow<CartridgeBuilderException>()
                .WithMessage($"Not enough space to reserve {allocation.Length} bytes at {allocation.Offset}");
        }

        [Test]
        public void Write_WithLowStrategy_WritesData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            var data = Enumerable.Range(0, 0x4000).Select(i => unchecked((byte) i)).ToArray();
            var allocation = new Allocation
            {
                Length = data.Length,
                Offset = 0x0000,
                WrapStrategy = WrapStrategy.Low
            };

            // Act.
            var output = Subject.Write(romSpace, data, allocation);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = allocation.Length,
                Offset = 0x0000,
                WrapStrategy = allocation.WrapStrategy
            });
            var expected = Enumerable.Repeat(UsageType.Used, 0x2000)
                .Concat(Enumerable.Repeat(UsageType.Unused, 0x2000))
                .Concat(Enumerable.Repeat(UsageType.Used, 0x2000))
                .Concat(Enumerable.Repeat(UsageType.Unused, 0x2000));
            romSpace.Usage.SequenceEqual(expected).Should().BeTrue();
        }

        [Test]
        public void Write_WithHighStrategy_WritesData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            var data = Enumerable.Range(0, 0x4000).Select(i => unchecked((byte) i)).ToArray();
            var allocation = new Allocation
            {
                Length = data.Length,
                Offset = 0x2000,
                WrapStrategy = WrapStrategy.High
            };

            // Act.
            var output = Subject.Write(romSpace, data, allocation);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = allocation.Length,
                Offset = 0x2000,
                WrapStrategy = allocation.WrapStrategy
            });
            var expected = Enumerable.Repeat(UsageType.Unused, 0x2000)
                .Concat(Enumerable.Repeat(UsageType.Used, 0x2000))
                .Concat(Enumerable.Repeat(UsageType.Unused, 0x2000))
                .Concat(Enumerable.Repeat(UsageType.Used, 0x2000));
            romSpace.Usage.SequenceEqual(expected).Should().BeTrue();
        }

        [Test]
        public void Write_WithBothStrategy_WritesData()
        {
            // Arrange.
            var romSpace = new RomSpace(0x8000);
            var data = Enumerable.Range(0, 0x4000).Select(i => unchecked((byte) i)).ToArray();
            var allocation = new Allocation
            {
                Length = data.Length,
                Offset = 0x2000,
                WrapStrategy = WrapStrategy.Both
            };

            // Act.
            var output = Subject.Write(romSpace, data, allocation);

            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                Length = allocation.Length,
                Offset = 0x2000,
                WrapStrategy = allocation.WrapStrategy
            });
            var expected = Enumerable.Repeat(UsageType.Unused, 0x2000)
                .Concat(Enumerable.Repeat(UsageType.Used, 0x4000))
                .Concat(Enumerable.Repeat(UsageType.Unused, 0x2000));
            romSpace.Usage.SequenceEqual(expected).Should().BeTrue();
        }

        [Test]
        public void Write_WithoutAvailableSpace_Throws()
        {
            // Arrange.
            var romSpace = new RomSpace(0x4000);
            var data = Enumerable.Repeat((byte) 0xFF, 0x8000).ToArray();

            var allocation = new Allocation
            {
                Length = data.Length,
                Offset = 0x0000,
                WrapStrategy = WrapStrategy.Both
            };

            // Act.
            Action act = () => Subject.Write(romSpace, data, allocation);

            // Assert.
            act.ShouldThrow<CartridgeBuilderException>()
                .WithMessage($"Not enough space to write {allocation.Length} bytes at {allocation.Offset}");
        }
    }
}