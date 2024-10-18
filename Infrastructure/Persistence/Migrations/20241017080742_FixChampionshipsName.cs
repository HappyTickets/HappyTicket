using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FixChampionshipsName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChampionSponsors_Championshipss_ChampionId",
                table: "ChampionSponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Championshipss_ChampionId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Championshipss",
                table: "Championshipss");

            migrationBuilder.RenameTable(
                name: "Championshipss",
                newName: "Championships");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Championships",
                table: "Championships",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChampionSponsors_Championships_ChampionId",
                table: "ChampionSponsors",
                column: "ChampionId",
                principalTable: "Championships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Championships_ChampionId",
                table: "Matches",
                column: "ChampionId",
                principalTable: "Championships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ChampionSponsors_Championships_ChampionId",
                table: "ChampionSponsors");

            migrationBuilder.DropForeignKey(
                name: "FK_Matches_Championships_ChampionId",
                table: "Matches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Championships",
                table: "Championships");

            migrationBuilder.RenameTable(
                name: "Championships",
                newName: "Championshipss");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Championshipss",
                table: "Championshipss",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ChampionSponsors_Championshipss_ChampionId",
                table: "ChampionSponsors",
                column: "ChampionId",
                principalTable: "Championshipss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Matches_Championshipss_ChampionId",
                table: "Matches",
                column: "ChampionId",
                principalTable: "Championshipss",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
