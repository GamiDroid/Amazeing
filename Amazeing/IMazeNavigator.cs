using Amazeing.Models;

namespace Amazeing;
public interface IMazeNavigator
{
    Task<PossibleActionsAndCurrentScore> EnterAsync(string mazeName);
    Task ExitAsync();
    Task<PossibleActionsAndCurrentScore> MoveAsync(Direction direction);
    Task<PossibleActionsAndCurrentScore> CollectAsync();
}
