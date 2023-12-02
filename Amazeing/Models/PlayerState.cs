namespace Amazeing.Models;
internal class PlayerState
{
    public int Id { get; set; }
    public string? Maze { get; set; }
    public int PositionX { get; set; }
    public int PositionY { get; set; }
    public Direction LastDirection { get; set; }
}
