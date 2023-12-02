using Amazeing.Models;
using Microsoft.EntityFrameworkCore;

namespace Amazeing;
internal class AmazeingDbContext : DbContext
{
    public DbSet<MazeInfo> Mazes { get; set; }
    public DbSet<PlayerState> PlayerStates { get; set; }
    public DbSet<MazeTile> MazeTiles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        options.UseSqlite("Data Source=./amazeing.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PlayerState>(playerState =>
        {
            playerState.Property(x => x.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<MazeTile>(mazeTile =>
        {
            mazeTile.HasAlternateKey(e => new { e.Maze, e.PositionX, e.PositionY });

            mazeTile.HasOne(e => e.MazeInfo)
                .WithMany(e => e.MazeTiles)
                .HasForeignKey(e => e.Maze);

            mazeTile.HasMany(e => e.Neighbours)
                .WithMany();
        });
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
