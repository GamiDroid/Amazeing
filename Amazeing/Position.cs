using Amazeing.Models;

namespace Amazeing;
internal record struct Position(int X, int Y)
{
    internal readonly Position Move(Direction direction)
    {
        int posX = direction switch
        {
            Direction.Right => X + 1,
            Direction.Left => X - 1,
            _ => X,
        };

        int posY = direction switch
        {
            Direction.Up => Y + 1,
            Direction.Down => Y - 1,
            _ => Y,
        };

        return new Position(posX, posY);
    }

    internal readonly Direction GetDirection(Position other)
    {
        if (other.X > this.X) return Direction.Right;
        else if (other.X < this.X) return Direction.Left;
        else if (other.Y > this.Y) return Direction.Up;
        else if (other.Y < this.Y) return Direction.Down;
        return Direction.None;
    }
}
