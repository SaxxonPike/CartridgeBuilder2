using System.Linq;
using CartridgeBuilder2.Lib.Builder;
using FluentAssertions;
using NUnit.Framework;

namespace CartridgeBuilder2.Lib.Test.Builder
{
    [TestFixture]
    public class IndexGeneratorFactoryTests : BaseUnitTestFixture<IndexGeneratorFactory, IIndexGeneratorFactory>
    {
        [Test]
        public void CreateGenerator_WithBothStrategy_GeneratesCorrectSequence()
        {
            // Arrange.
            var expected = Enumerable.Range(0, 0x8000);
            
            // Act.
            var output = Subject.CreateGenerator(WrapStrategy.Both)(0, 0x8000);
            
            // Assert.
            output.SequenceEqual(expected).Should().BeTrue();
        }
        
        [Test]
        public void CreateGenerator_WithLowStrategy_GeneratesCorrectSequence()
        {
            // Arrange.
            var expected = Enumerable.Range(0, 0x2000)
                .Concat(Enumerable.Range(0x4000, 0x2000));
            
            // Act.
            var output = Subject.CreateGenerator(WrapStrategy.Low)(0, 0x8000);
            
            // Assert.
            output.SequenceEqual(expected).Should().BeTrue();
        }

        [Test]
        public void CreateGenerator_WithHighStrategy_GeneratesCorrectSequence()
        {
            // Arrange.
            var expected = Enumerable.Range(0x2000, 0x2000)
                .Concat(Enumerable.Range(0x6000, 0x2000));
            
            // Act.
            var output = Subject.CreateGenerator(WrapStrategy.High)(0, 0x8000);
            
            // Assert.
            output.SequenceEqual(expected).Should().BeTrue();
        }
        
    }
}