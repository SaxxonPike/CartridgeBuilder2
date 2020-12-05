using CartridgeBuilder2.Lib.Builder;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Builder
{
    [TestFixture]
    public class AllocationGeneratorTests : BaseUnitTestFixture<AllocationGenerator, IAllocationGenerator>
    {
        [Test]
        public void GenerateFromPatch_ShouldHaveCorrectValues()
        {
            // Arrange.
            var patch = Create<Patch>();
            
            // Act.
            var output = Subject.GenerateFromPatch(patch);
            
            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                CompressionMethod = default(CompressionMethod),
                Length = patch.Data.Length,
                Offset = patch.Offset + patch.Bank * 0x4000,
                WrapStrategy = patch.WrapStrategy
            });
        }
        
        [Test]
        public void GenerateFromTable_ShouldHaveCorrectValues()
        {
            // Arrange.
            var table = Create<Table>();
            
            // Act.
            var output = Subject.GenerateFromTable(table);
            
            // Assert.
            output.Should().BeEquivalentTo(new Allocation
            {
                CompressionMethod = default(CompressionMethod),
                Length = table.Length,
                Offset = table.Offset + table.Bank * 0x4000,
                WrapStrategy = table.WrapStrategy
            });
        }
    }
}