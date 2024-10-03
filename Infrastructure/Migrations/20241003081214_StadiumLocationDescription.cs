using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class StadiumLocationDescription : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "location",
                table: "Stadiums",
                newName: "Location");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Stadiums",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LocationUrl",
                table: "Stadiums",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "LocationUrl",
                table: "Stadiums");

            migrationBuilder.RenameColumn(
                name: "Location",
                table: "Stadiums",
                newName: "location");
        }
    }
}
