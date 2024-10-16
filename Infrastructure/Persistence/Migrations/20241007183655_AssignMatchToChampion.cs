using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AssignMatchToChampion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ChampionId",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Matches_ChampionId",
                table: "Matches",
                column: "ChampionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Champions_ChampionId",
                table: "Matches",
                column: "ChampionId",
                principalTable: "Champions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Champions_ChampionId",
                table: "Matches");

            migrationBuilder.DropIndex(
                name: "IX_Matches_ChampionId",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ChampionId",
                table: "Matches");
        }
    }
}
