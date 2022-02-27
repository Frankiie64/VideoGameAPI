using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace PVideoGamesAPI.Migrations
{
    public partial class RequerimentsMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {       
            migrationBuilder.CreateTable(
                name: "requeriments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Os = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Graphics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DirectX = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Storage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requeriments", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "requeriments");

            migrationBuilder.CreateTable(
                name: "requirement",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DirectX = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Graphics = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Memory = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Os = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Processor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Storage = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_requirement", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "videoGame",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Developers = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IdCategory = table.Column<int>(type: "int", nullable: false),
                    IdRequirements = table.Column<int>(type: "int", nullable: false),
                    ImagenRoute = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Platforms = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Release_Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Sumary = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    clasificacion = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_videoGame", x => x.Id);
                    table.ForeignKey(
                        name: "FK_videoGame_category_IdCategory",
                        column: x => x.IdCategory,
                        principalTable: "category",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_videoGame_requirement_IdRequirements",
                        column: x => x.IdRequirements,
                        principalTable: "requirement",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_videoGame_IdCategory",
                table: "videoGame",
                column: "IdCategory");

            migrationBuilder.CreateIndex(
                name: "IX_videoGame_IdRequirements",
                table: "videoGame",
                column: "IdRequirements");
        }
    }
}
