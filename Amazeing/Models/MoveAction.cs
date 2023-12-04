namespace Amazeing.Models;
internal sealed class MoveAction
{
    public string? Direction { get; set; }
    public bool IsStart { get; set; }
    public bool AllowsExit { get; set; }
    public bool AllowsScoreCollection { get; set; }
    public bool HasBeenVisited { get; set; }
    public int RewardOnDestination { get; set; }
    public long? TagOnTile { get; set; }
}

internal enum Direction { None, Up, Right, Down, Left }
