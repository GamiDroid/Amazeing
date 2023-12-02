using Amazeing;
using Amazeing.Models;
﻿
AmazeingDbContext.CreateOrUpdateModel();

using var httpclient = new HttpClient
{
    BaseAddress = new Uri("https://maze.hightechict.nl/"),
};

httpclient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", "HTI Thanks You [9aA]");

var response = await httpclient.GetAsync("/api/mazes/all");

response.EnsureSuccessStatusCode();

var stringContent = await response.Content.ReadAsStringAsync();

Console.WriteLine(stringContent);

Console.ReadLine();