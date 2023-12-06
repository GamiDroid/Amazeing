using Amazeing.Models;
using Amazeing.Repositories;

namespace Amazeing;
internal sealed class MazeSolver
{
    private readonly MazeRepo _repo;
    private readonly IMazeNavigator _mazeNavigator;
    private MazePlayerState _state = new();

    public MazeSolver(MazeRepo repo, IMazeNavigator mazeNavigator)
    {
        _repo = repo;
        _mazeNavigator = mazeNavigator;
    }

    public async Task EnterAsync(string mazeName)
    {
        var mazeInfo = await _repo.GetMazeByNameAsync(mazeName) ??
            throw new InvalidDataException($"Could not find maze with name {mazeName}");

        var possibleActionsAndCurrentScore = await _mazeNavigator.EnterAsync(mazeName);

        await Console.Out.WriteLineAsync($"Entering maze '{mazeName}'");

        _state = new MazePlayerState
        {
            TotalTiles = mazeInfo.TotalTiles,
            PotentialReward = mazeInfo.PotentialReward
        };

        UpdateMazePlayerState(possibleActionsAndCurrentScore, Direction.None);
    }

    public async Task SolveAsync()
    {
        while (true)
        {
            if (_state.MazeScoreInBag == _state.PotentialReward)
            {
                // try going to exit...

                var currentTile = _state.Tiles.First(x => x.Position == _state.PlayerPosition);
                if (currentTile.ExitPoint)
                {
                    await ExitAsync();
                    break; // Maze solved, exit while-loop
                }

                Position positionToMoveTo;
                var exitPoint = _state.Tiles.FirstOrDefault(x => x.ExitPoint);
                if (exitPoint is null)
                {
                    var notVisitedTile = _state.Tiles.First(x => x.IsVisited == false);
                    positionToMoveTo = notVisitedTile.Position;
                }
                else
                {
                    positionToMoveTo = exitPoint.Position;
                }

                await MoveToPositionAsync(positionToMoveTo);
            }
            else if (_state.MazeScoreInHand == _state.PotentialReward)
            {
                // try going to collect position...

                var currentTile = _state.Tiles.First(x => x.Position == _state.PlayerPosition);
                if (currentTile.CollectionPoint)
                {
                    await CollectAsync();
                    continue;
                }

                Position positionToMoveTo;
                var collectionPoint = _state.Tiles.FirstOrDefault(x => x.CollectionPoint);
                if (collectionPoint is null)
                {
                    var notVisitedTile = _state.Tiles.First(x => x.IsVisited == false);
                    positionToMoveTo = notVisitedTile.Position;
                }
                else
                {
                    positionToMoveTo = collectionPoint.Position;
                }
                
                await MoveToPositionAsync(positionToMoveTo);
            }
            else if (_state.AmountOfTilesFound == _state.TotalTiles)
            {
                // Go to tile with reward
                var tileWithReward = _state.Tiles.FirstOrDefault(x => x.Reward > 0);
                if (tileWithReward is not null)
                {
                    await MoveToPositionAsync(tileWithReward.Position);
                }
            }
            else
            {
                // move to next tile...

                var possiblePositions = _state.Paths
                    .Where(x => x.Start == _state.PlayerPosition)
                    .Select(x => x.End)
                    .ToList();

                var tile = _state.Tiles
                    .Where(x => possiblePositions.Contains(x.Position)
                        && x.IsVisited == false)
                    .OrderByDescending(x => x.Reward)
                    .FirstOrDefault();

                if (tile is not null)
                {
                    var direction = _state.PlayerPosition.GetDirection(tile.Position);
                    await MoveAsync(direction);
                }
                else
                {
                    var notVisitedTile = _state.Tiles
                        .Where(x => x.IsVisited == false)
                        .OrderByDescending(x => x.Reward)
                        .First();
                    await MoveToPositionAsync(notVisitedTile.Position);
                }
            }
        }
    }

    private ICollection<Direction> FindPathToDestination(Position start, Position destination)
    {
        if (start == destination)
        {
            return Array.Empty<Direction>();
        }
        
        var possiblePaths = _state.Paths.Where(x => x.Start == start);
        if (!possiblePaths.Any())
        {
            throw new InvalidOperationException("Cannot move from this position");
        }

        var queue = new PriorityQueue<(Position, ICollection<Direction>), int>();
        foreach (var path in possiblePaths)
        {
            var direction = path.Start.GetDirection(path.End);
            queue.Enqueue((path.End, new[] { direction }), 1);
        }

        while (queue.TryDequeue(out var i, out var _))
        {
            var currentPos = i.Item1;
            var directions = i.Item2;

            if (currentPos == destination)
                return directions;

            foreach (var path in _state.Paths.Where(x => x.Start == currentPos))
            {
                var direction = path.Start.GetDirection(path.End);
                var newDirections = new List<Direction>(directions) { direction };
                queue.Enqueue((path.End, newDirections), newDirections.Count);
            }
        }

        return Array.Empty<Direction>();
    }

    public async Task MoveAsync(Direction direction)
    {
        await Console.Out.WriteLineAsync($"Trying to move {direction}...");

        var possibleActionsAndCurrentScore = await _mazeNavigator.MoveAsync(direction);

        UpdateMazePlayerState(possibleActionsAndCurrentScore, direction);

        await Console.Out.WriteLineAsync($"Moved {direction} to {_state.PlayerPosition}");
    }

    public async Task MoveToPositionAsync(Position destination)
    {
        await Console.Out.WriteLineAsync($"Try moving from {_state.PlayerPosition} to {destination}");

        var directionsToDestination = FindPathToDestination(_state.PlayerPosition, destination);
        foreach (var direction in directionsToDestination)
        {
            await MoveAsync(direction);
        }

        await Console.Out.WriteLineAsync($"Player arrived at {_state.PlayerPosition}");
    }

    public async Task CollectAsync()
    {
        await Console.Out.WriteLineAsync("Trying to collect score...");

        var possibleActionsAndCurrentScore = await _mazeNavigator.CollectAsync();

        UpdateMazePlayerState(possibleActionsAndCurrentScore, Direction.None);

        await Console.Out.WriteLineAsync("Score collected");
    }

    public async Task ExitAsync()
    {
        await Console.Out.WriteLineAsync("Trying to exit maze...");
        await _mazeNavigator.ExitAsync();
        await Console.Out.WriteLineAsync("Exited the maze");
    }

    private void UpdateMazePlayerState(PossibleActionsAndCurrentScore possibleActionsAndCurrentScore, Direction direction)
    {
        _state.MazeScoreInHand = possibleActionsAndCurrentScore.CurrentScoreInHand;
        _state.MazeScoreInBag = possibleActionsAndCurrentScore.CurrentScoreInBag;

        _state.MovePlayer(direction);

        var tile = new MazeTile
        {
            Position = _state.PlayerPosition,
            IsVisited = true,
            CollectionPoint = possibleActionsAndCurrentScore.CanCollectScoreHere,
            ExitPoint = possibleActionsAndCurrentScore.CanExitMazeHere,
            Reward = 0,
            Tag = possibleActionsAndCurrentScore.TagOnCurrentTile,
        };

        _state.AddOrUpdateMazeTile(tile);

        foreach (var action in possibleActionsAndCurrentScore.PossibleMoveActions)
        {
            var possibleDirection = Enum.Parse<Direction>(action.Direction ?? "None", true);
            var neighborTile = new MazeTile
            {
                Position = _state.PlayerPosition.Move(possibleDirection),
                IsVisited = action.HasBeenVisited,
                CollectionPoint = action.AllowsScoreCollection,
                ExitPoint = action.AllowsExit,
                Reward = action.RewardOnDestination,
                Tag = action.TagOnTile,
            };

            _state.AddOrUpdateMazeTile(neighborTile);
            _state.AddPath(tile.Position, neighborTile.Position);
        }
    }
}

internal class MazePlayerState
{
    public Position PlayerPosition { get; set; }
    public int TotalTiles { get; set; }
    public int PotentialReward { get; set; }
    public int MazeScoreInHand { get; set; }
    public int MazeScoreInBag { get; set; }

    public ICollection<MazeTile> Tiles { get; } = new HashSet<MazeTile>();
    public int AmountOfTilesFound => Tiles.Count;
    public void AddOrUpdateMazeTile(MazeTile tile)
    {
        var existingTile = Tiles.FirstOrDefault(t => t.Position == tile.Position);
        if (existingTile is null)
        {
            Tiles.Add(tile);
        }
        else
        {
            existingTile.IsVisited = tile.IsVisited;
            existingTile.Reward = tile.Reward;
            existingTile.Tag = tile.Tag;
        }
    }

    public ICollection<(Position Start, Position End)> Paths { get; } = new HashSet<(Position Start, Position End)>();
    public void AddPath(Position start, Position end)
    {
        Paths.Add((start, end));
        Paths.Add((end, start));
    }

    public void MovePlayer(Direction direction)
    {
        PlayerPosition = PlayerPosition.Move(direction);
    }
}
