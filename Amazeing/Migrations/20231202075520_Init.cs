using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Amazeing.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Mazes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Name = table.Column<string>(type: "TEXT", nullable: true),
                    TotalTiles = table.Column<int>(type: "INTEGER", nullable: false),
                    PotentialReward = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Mazes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStates",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false),
                    Maze = table.Column<string>(type: "TEXT", nullable: true),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: false),
                    LastDirection = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MazeTiles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Maze = table.Column<Guid>(type: "TEXT", nullable: false),
                    PositionX = table.Column<int>(type: "INTEGER", nullable: false),
                    PositionY = table.Column<int>(type: "INTEGER", nullable: false),
                    IsVisited = table.Column<bool>(type: "INTEGER", nullable: false),
                    Tag = table.Column<long>(type: "INTEGER", nullable: false),
                    TileType = table.Column<char>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MazeTiles", x => x.Id);
                    table.UniqueConstraint("AK_MazeTiles_Maze_PositionX_PositionY", x => new { x.Maze, x.PositionX, x.PositionY });
                    table.ForeignKey(
                        name: "FK_MazeTiles_Mazes_Maze",
                        column: x => x.Maze,
                        principalTable: "Mazes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MazeTileMazeTile",
                columns: table => new
                {
                    MazeTileId = table.Column<int>(type: "INTEGER", nullable: false),
                    NeighboursId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MazeTileMazeTile", x => new { x.MazeTileId, x.NeighboursId });
                    table.ForeignKey(
                        name: "FK_MazeTileMazeTile_MazeTiles_MazeTileId",
                        column: x => x.MazeTileId,
                        principalTable: "MazeTiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MazeTileMazeTile_MazeTiles_NeighboursId",
                        column: x => x.NeighboursId,
                        principalTable: "MazeTiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MazeTileMazeTile_NeighboursId",
                table: "MazeTileMazeTile",
                column: "NeighboursId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MazeTileMazeTile");

            migrationBuilder.DropTable(
                name: "PlayerStates");

            migrationBuilder.DropTable(
                name: "MazeTiles");

            migrationBuilder.DropTable(
                name: "Mazes");
        }
    }
}
