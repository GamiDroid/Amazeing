using Amazeing;
using Amazeing.Models;
using Amazeing.Repositories;
using Spectre.Console;
using System.Net.Http.Headers;
using System.Net.Http.Json;

AmazeingDbContext.CreateOrUpdateModel();

var httpclient = new HttpClient
{
    BaseAddress = new Uri("https://maze.hightechict.nl/"),
};
httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "HTI Thanks You [9aA]");

using var mazeRepo = new MazeRepo(httpclient);

var mazes = await mazeRepo.GetAllMazeInfosAsync();

var grid = new Grid();
grid.AddColumns(3);

grid.AddRow(nameof(MazeInfo.Name), nameof(MazeInfo.TotalTiles), nameof(MazeInfo.PotentialReward));

foreach (var maze in mazes)
{
    grid.AddRow(maze.Name ?? "", maze.TotalTiles.ToString(), maze.PotentialReward.ToString());
}

AnsiConsole.Write(grid);

var mazeSolver = new MazeSolver(mazeRepo, httpclient);

try
{
    await mazeSolver.EnterAsync("Test");
    await mazeSolver.SolveAsync();

    await mazeSolver.EnterAsync("Example Maze");
    await mazeSolver.SolveAsync();

    await mazeSolver.EnterAsync("Hello Maze");
    await mazeSolver.SolveAsync();
}
catch (Exception ex)
{
    Console.WriteLine("Unhandled exception: {0}", ex);
}
finally
{
    Console.WriteLine("End of program. Press any key to exit");
    Console.ReadKey();
}
