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
}
