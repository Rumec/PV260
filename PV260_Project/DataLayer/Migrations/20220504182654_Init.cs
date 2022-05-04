using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLayer.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DataSets",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Date = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DataSets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Holding",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Shares = table.Column<long>(type: "INTEGER", nullable: false),
                    MarketValue = table.Column<double>(type: "REAL", nullable: false),
                    Weight = table.Column<double>(type: "REAL", nullable: false),
                    Currency = table.Column<string>(type: "TEXT", nullable: false),
                    DataSetId = table.Column<int>(type: "INTEGER", nullable: true),
                    Fund = table.Column<string>(type: "TEXT", nullable: false),
                    Company = table.Column<string>(type: "TEXT", nullable: false),
                    Ticker = table.Column<string>(type: "TEXT", nullable: false),
                    Cusip = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holding", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holding_DataSets_DataSetId",
                        column: x => x.DataSetId,
                        principalTable: "DataSets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holding_DataSetId",
                table: "Holding",
                column: "DataSetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holding");

            migrationBuilder.DropTable(
                name: "DataSets");
        }
    }
}
