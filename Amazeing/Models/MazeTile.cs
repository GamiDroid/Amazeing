namespace Amazeing.Models;
internal class MazeTile
{
    public int Id { get; set; }
    public Guid Maze { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public bool IsVisited { get; set; }
    public long Tag { get; set; }
    public char TileType { get; set; }

    public MazeInfo? MazeInfo { get; set; }
    public ICollection<MazeTile> Neighbours { get; set; } = [];

    public override string ToString()
    {
        return $"[{PositionX},{PositionY}]: {TileType}";
    }
}
