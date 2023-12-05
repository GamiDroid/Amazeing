namespace Amazeing.Models;
internal sealed class MazeInfo
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? Name { get; set; }
    public int TotalTiles { get; set; }
    public int PotentialReward { get; set; }
}
