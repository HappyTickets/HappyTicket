using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class IsOver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Tickets");

            migrationBuilder.AddColumn<string>(
                name: "Class",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "SeatsNumber",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsOver",
                table: "Matches",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Class",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SeatsNumber",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsOver",
                table: "Matches");

            migrationBuilder.AddColumn<string>(
                name: "BuyerId",
                table: "Tickets",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
