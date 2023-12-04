using Amazeing.Models;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace Amazeing.Repositories;
internal class MazeRepo : IDisposable
{
    private readonly HttpClient _httpClient;

    public MazeRepo(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ICollection<MazeInfo>> GetAllMazeInfosAsync()
    {
        using var db = new AmazeingDbContext();

        var mazes = await db.Mazes.AsNoTracking().ToListAsync();

        if (mazes.Count == 0)
        {
            mazes = await _httpClient.GetFromJsonAsync<List<MazeInfo>>("/api/mazes/all") ?? [];

            if (mazes.Count != 0)
            {
                // Add maze infos to local storage
                db.Mazes.AddRange(mazes);
                await db.SaveChangesAsync();
            }
        }

        return mazes;
    }

    public Task<MazeInfo?> GetMazeByNameAsync(string name)
    {
        using var db = new AmazeingDbContext();
        return db.Mazes.AsNoTracking().FirstOrDefaultAsync(x => x.Name == name);
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}
