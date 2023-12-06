using Amazeing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Amazeing;
internal class MazeNavigator : IMazeNavigator
{
    private readonly HttpClient _httpClient;

    public MazeNavigator(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PossibleActionsAndCurrentScore> EnterAsync(string mazeName)
    {
        var response = await _httpClient.PostAsync($"/api/mazes/enter?mazeName={mazeName}", new StringContent(""));
        response.EnsureSuccessStatusCode();

        var possibleActionsAndCurrentScore = await response.Content.ReadFromJsonAsync<PossibleActionsAndCurrentScore>() ??
            throw new InvalidDataException($"response could not be parsed to type {nameof(PossibleActionsAndCurrentScore)}");

        return possibleActionsAndCurrentScore;
    }

    public async Task ExitAsync()
    {
        var response = await _httpClient.PostAsync("/api/maze/exit", new StringContent(""));
        response.EnsureSuccessStatusCode();
    }

    public async Task<PossibleActionsAndCurrentScore> MoveAsync(Direction direction)
    {
        var response = await _httpClient.PostAsync($"/api/maze/move?direction={direction}", new StringContent(""));
        response.EnsureSuccessStatusCode();

        var possibleActionsAndCurrentScore = await response.Content.ReadFromJsonAsync<PossibleActionsAndCurrentScore>()
                ?? throw new InvalidDataException();

        return possibleActionsAndCurrentScore;
    }

    public async Task<PossibleActionsAndCurrentScore> CollectAsync()
    {
        var response = await _httpClient.PostAsync("/api/maze/collectScore", new StringContent(""));
        response.EnsureSuccessStatusCode();

        var possibleActionsAndCurrentScore = await response.Content.ReadFromJsonAsync<PossibleActionsAndCurrentScore>()
            ?? throw new InvalidDataException();

        return possibleActionsAndCurrentScore;
    }
}
