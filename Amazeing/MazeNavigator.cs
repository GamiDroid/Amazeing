using Amazeing.Models;
using Amazeing.Repositories;
using System.Net.Http.Json;

namespace Amazeing;
internal sealed class MazeNavigator
{
    private readonly MazeRepo _repo;
    private readonly HttpClient _httpClient;
    private MazePlayerState _state = new();

    public MazeNavigator(MazeRepo repo, HttpClient httpClient)
    {
        _repo = repo;
        _httpClient = httpClient;
    }

    public async Task EnterAsync(string mazeName)
    {
        var mazeInfo = await _repo.GetMazeByNameAsync(mazeName) ??
            throw new InvalidDataException($"Could not find maze with name {mazeName}");

        var response = await _httpClient.PostAsync($"/api/mazes/enter?mazeName={mazeName}", new StringContent(""));
        response.EnsureSuccessStatusCode();

        _state = new MazePlayerState
        {
            TotalTiles = mazeInfo.TotalTiles,
            PotentialReward = mazeInfo.PotentialReward
        };

        var possibleActionsAndCurrentScore = await response.Content.ReadFromJsonAsync<PossibleActionsAndCurrentScore>() ??
            throw new InvalidDataException($"response could not be parsed to type {nameof(PossibleActionsAndCurrentScore)}");

        UpdateMazePlayerState(possibleActionsAndCurrentScore, Direction.None);
    }

    public async Task MoveAsync(Direction direction)
    {
        await Console.Out.WriteLineAsync($"Trying to move {direction}...");

        var response = await _httpClient.PostAsync($"/api/maze/move?direction={direction}", new StringContent(""));
        response.EnsureSuccessStatusCode();    

        var possibleActionsAndCurrentScore = await response.Content.ReadFromJsonAsync<PossibleActionsAndCurrentScore>()
                ?? throw new InvalidDataException();

        UpdateMazePlayerState(possibleActionsAndCurrentScore, direction);

        await Console.Out.WriteLineAsync($"Moved {direction}");
    }

    public async Task CollectionAsync()
    {
        await Console.Out.WriteLineAsync("Trying to collect score...");

        var response = await _httpClient.PostAsync("/api/maze/collectScore", new StringContent(""));
        response.EnsureSuccessStatusCode();

        var possibleActionsAndCurrentScore = await response.Content.ReadFromJsonAsync<PossibleActionsAndCurrentScore>() 
            ?? throw new InvalidDataException();

        UpdateMazePlayerState(possibleActionsAndCurrentScore, Direction.None);

        await Console.Out.WriteLineAsync("Score collected");
    }

    public async Task ExitAsync()
    {
        await Console.Out.WriteLineAsync("Trying to exit maze...");

        var response = await _httpClient.PostAsync("/api/maze/exit", new StringContent(""));
        response.EnsureSuccessStatusCode();

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
    public int TilesFound => Tiles.Count;
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
