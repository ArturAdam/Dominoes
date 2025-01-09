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
                    new object[]
                    {
                        new List<Domino> { new(6, 5), new(6, 5) }
                    }, // Two dominoes forming a valid circle
                    new object[]
                    {
                        new List<Domino>
                        {
                            new(1, 1), new(1, 1), new(1, 2), new(2, 2), new(2, 1), new(1, 1)
                        } // For good measure
                    },
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

            [Fact]
            public void CanFormCircle_ValidCircle_ReturnsExpectedChain()
            {
                // Arrange
                var dominoes = new List<Domino>() { new(1, 2), new(2, 2), new(1, 2) };
                var graph = new DominoGraph(dominoes);

                // Act
                var chain = graph.FormCircle();

                // Assert
                Assert.Collection(chain.AsT0,
                    domino =>
                    {
                        Assert.Equal<int>(1, domino.Left);
                        Assert.Equal<int>(2, domino.Right);
                    },
                    domino =>
                    {
                        Assert.Equal<int>(2, domino.Left);
                        Assert.Equal<int>(2, domino.Right);
                    },
                    domino =>
                    {
                        Assert.Equal<int>(2, domino.Left);
                        Assert.Equal<int>(1, domino.Right);
                    }
                );
            }

            [Fact]
            public void CanFormCircle_ValidCircle_ReturnsExpectedChainInCorrectOrder()
            {
                // Arrange
                var dominoes = new List<Domino>() { new(1, 3), new(4, 3), new(1, 3), new(3, 4) };
                var graph = new DominoGraph(dominoes);

                // Act
                var chain = graph.FormCircle();

                //Assert
                Assert.Collection<Domino>(chain.AsT0,
                    domino =>
                    {
                        Assert.Equal<int>(1, domino.Left);
                        Assert.Equal<int>(3, domino.Right);
                    },
                    domino =>
                    {
                        Assert.Equal<int>(3, domino.Left);
                        Assert.Equal<int>(4, domino.Right);
                    },
                    domino =>
                    {
                        Assert.Equal<int>(4, domino.Left);
                        Assert.Equal<int>(3, domino.Right);
                    },
                    domino =>
                    {
                        Assert.Equal<int>(3, domino.Left);
                        Assert.Equal<int>(1, domino.Right);
                    }
                );
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
            [InlineData(7, 7)]
            [InlineData(-1, -1)]
            [InlineData(1000, -5)]
            [InlineData(-10, 10000)]
            public void DominoGraph_InvalidDomino_Throws(int left, int right)
            {
                // Arrange,Act & Assert
                Assert.Throws<ArgumentOutOfRangeException>(() => new Domino(left, right));
            }
        }
    }
}