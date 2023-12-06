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

var mazeNavigator = new MazeNavigator(httpclient);
var mazeSolver = new MazeSolver(mazeRepo, mazeNavigator);

var mazeNames = new[] 
{
    "Dig Down",
    "Easy deal",
    "Egg",
    "Example Maze",
    "Exit",
    "Glasses",
    "Gradius Pathways",
    "Hello Maze",
    "Loops",
    "Michiel",
    "O Contra",
    "PacMan",
    "Reverse",
    "Spiral Of Doom",
    "Test",
    "Void",
    "Needle",
};

try
{
    foreach (var mazeName in mazeNames)
    {
        try
        {
            await mazeSolver.EnterAsync(mazeName);
            await mazeSolver.SolveAsync();
        }
        catch
        {
            Console.WriteLine("Exception occured. state has been saved to json file.");
        }
    }
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
