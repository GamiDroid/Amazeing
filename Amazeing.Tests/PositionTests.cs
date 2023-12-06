using Amazeing.Models;

namespace Amazeing.Tests;

public class PositionTests
{
    [Fact]
    public void NewPosition_ShouldBeZeroZero()
    {
        // arrange
        Position p = new();

        // assert
        Assert.Equal(0, p.X);
        Assert.Equal(0, p.Y);
    }

    [Theory]
    [InlineData(Direction.None, 0, 0)]
    [InlineData(Direction.Up, 0, 1)]
    [InlineData(Direction.Down, 0, -1)]
    [InlineData(Direction.Right, 1, 0)]
    [InlineData(Direction.Left, -1, 0)]
    public void Move_ShouldReturnPositionRelativeToPosition(Direction direction, int expectedX, int expectedY)
    {
        // arrange
        Position p1 = new();

        // act
        var p2 = p1.Move(direction);

        // assert
        Assert.Equal(expectedX, p2.X);
        Assert.Equal(expectedY, p2.Y);
    }
}