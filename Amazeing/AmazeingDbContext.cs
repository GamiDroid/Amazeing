using Amazeing.Models;
using Microsoft.EntityFrameworkCore;

namespace Amazeing;
internal class AmazeingDbContext : DbContext
{
    public DbSet<MazeInfo> Mazes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=./amazeing.db");
    }

    public static void CreateOrUpdateModel()
    {
        using var db = new AmazeingDbContext();

        db.Database.EnsureCreated();
        if (db.Database.HasPendingModelChanges())
        {
            db.Database.Migrate();
        }
    }
}
