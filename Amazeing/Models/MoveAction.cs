namespace Amazeing.Models;
internal sealed class MoveAction
{
    public Direction Direction { get; set; }
    public bool IsStart { get; set; }
    public bool AllowsExit { get; set; }
    public bool AllowsScoreCollection { get; set; }
    public bool HasBeenVisited { get; set; }
    public int RewardOnDestination { get; set; }
    public long TagOnTile { get; set; }
}

internal enum Direction { Up, Right, Down, Left }
