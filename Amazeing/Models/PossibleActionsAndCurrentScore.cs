namespace Amazeing.Models;
internal sealed class PossibleActionsAndCurrentScore
{
    public HashSet<MoveAction> PossibleMoveActions { get; set; } = [];
    public bool CanCollectScoreHere { get; set; }
    public bool CanExitMazeHere { get; set; }
    public int CurrentScoreInHand { get; set; }
    public int CurrentScoreInBag { get; set; }
    public long TagOnCurrentTile { get; set; }
}
