using Dominoes;
using Dominoes.Model;

namespace DominoesTests
{
    namespace Dominoes.Tests
    {
        public class DominoChainTests
        {
            public static IReadOnlyList<object[]> GetDominoChainTestDataPositive()
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        new List<Domino> { new(1, 2), new(2, 3), new(3, 1) }
                    }, // Simple circular chain

                    new object[]
                    {
                        new List<Domino> { new(2, 2), new(2, 3), new(3, 2) }
                    }, // Circular chain with a double

                    new object[]
                    {
                        new List<Domino> { new(4, 4) }
                    }, // Single double, circular chain (trivially complete)
                                new object[]
                    {
                     new List<Domino> { new(4, 4), new(4, 4), new(4, 4), new(4, 4) , new(4, 4) }
                    }, // Multiple doubles, circular chain
                    new object[] {
                        new List<Domino> { new(5, 4), new(4, 4), new(4, 5), new(4, 4) , new(4, 4) }
                    }, // Multiple double pairs, circular chain
                    new object[]
                    {
                        new List<Domino> { new(5, 6), new(6, 5) }
                    }, // Two dominoes forming a valid circle
                };
            }

            public static IReadOnlyList<object[]> GetDominoChainTestDataNegative()
            {
                return new List<object[]>
                {
                    new object[]
                    {
                        new List<Domino> { new(1, 2), new(2, 3), new(3, 4) }
                    }, // Not circular, ends don't match

                    new object[]
                    {
                        new List<Domino> { new(1, 2), new(3, 4) }
                    }, // Disconnected components

                    new object[]
                    {
                        new List<Domino> { new(1, 2), new(2, 3), new(3, 3) }
                    }, // Odd degree issue with 3

                    new object[]
                    {
                        new List<Domino> { new(1, 1), new(2, 2) }
                    }, // Isolated doubles with no connections

                    new object[]
                    {
                        new List<Domino> { new(1, 2) }
                    }, // Single non-double domino (cannot close a circle)
                    new object[]{
                        new List<Domino>{ }
                    }
                    ,
                     new object[] {
                        new List<Domino> { new(5, 5), new(4, 4), new(5, 5), new(4, 4) , new(4, 4) }
                    }, // Multiple double pairs, disconnected
                };
            }

            [Theory]
            [MemberData(nameof(GetDominoChainTestDataPositive))]
            public void CanFormCircle_ValidCircle_ReturnsResultList(List<Domino> input)
            {
                // Arrange
                var dominoChain = new DominoGraph(input);

                // Act
                var result = dominoChain.FormCircle();

                // Assert
                Assert.NotEmpty(result.AsT0);
            }

            [Theory]
            [MemberData(nameof(GetDominoChainTestDataNegative))]
            public void CanFormCircle_InvalidCircle_ReturnsErrorString(List<Domino> input)
            {
                // Arrange
                var dominoChain = new DominoGraph(input);

                // Act
                var result = dominoChain.FormCircle();

                // Assert
                Assert.NotEmpty(result.AsT1.Value);
            }

            [Theory]
            [InlineData(0, 7)]
            [InlineData(-1, 0)]
            public void DominoGraph_InvalidDomino_Throws(int left, int right)
            {
                // Arrange,Act & Assert
                Assert.Throws<ArgumentOutOfRangeException>(() => new Domino(left, right));
            }
        }
    }
}