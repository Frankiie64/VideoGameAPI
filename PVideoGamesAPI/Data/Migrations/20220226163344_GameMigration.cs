using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PVideoGamesAPI.Migrations
{
    public partial class GameMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Game",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagenRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Sumary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Release_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    clasificacion = table.Column<int>(type: "int", nullable: false),
                    Developers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCategory = table.Column<int>(type: "int", nullable: false),
                    IdRequirements = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Game", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Game_category_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Game_requeriments_IdRequirements",
                        column: x => x.IdRequirements,
                        principalTable: "requeriments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Game_IdCategory",
                table: "Game",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_Game_IdRequirements",
                table: "Game",
                column: "IdRequirements");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Game");
        }
    }
}
