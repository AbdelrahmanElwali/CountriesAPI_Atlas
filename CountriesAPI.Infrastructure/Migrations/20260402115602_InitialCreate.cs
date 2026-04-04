using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountriesAPI.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommonName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    OfficialName = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Cca2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Cca3 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: false),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Subregion = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Capital = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Population = table.Column<long>(type: "bigint", nullable: false),
                    FlagEmoji = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    FlagPng = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Lat = table.Column<double>(type: "float", nullable: false),
                    Lng = table.Column<double>(type: "float", nullable: false),
                    RawJson = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastSyncedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Cca2",
                table: "Countries",
                column: "Cca2",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Countries_Cca3",
                table: "Countries",
                column: "Cca3",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
