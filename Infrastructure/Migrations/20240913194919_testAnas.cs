using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class testAnas : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefreshTokens_BaseEntity_Id",
                table: "RefreshTokens");

            migrationBuilder.DropForeignKey(
                name: "FK_UserFavoriteTeams_BaseEntity_Id",
                table: "UserFavoriteTeams");

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "UserFavoriteTeams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "UserFavoriteTeams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "UserFavoriteTeams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "UserFavoriteTeams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "UserFavoriteTeams",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "UserFavoriteTeams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "UserFavoriteTeams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "Tickets",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Tickets",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Tickets",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Tickets",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Tickets",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "Tickets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "Teams",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Teams",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Teams",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Teams",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Teams",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Teams",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "Teams",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "Stadiums",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Stadiums",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Stadiums",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Stadiums",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Stadiums",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Stadiums",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "Stadiums",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "Seats",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Seats",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Seats",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Seats",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Seats",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Seats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "Seats",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "RefreshTokens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "RefreshTokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "RefreshTokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "RefreshTokens",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "RefreshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Orders",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "Matches",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Matches",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Matches",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Matches",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Matches",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "Matches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "CartItems",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "CartItems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CartItems",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "CartItems",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "CartItems",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "CartItems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "Cart",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Cart",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Cart",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Cart",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Cart",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Cart",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "Cart",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BaseEntityStatus",
                table: "Blocks",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CreatedBy",
                table: "Blocks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "Blocks",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Blocks",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ModifiedBy",
                table: "Blocks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "Blocks",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SoftDeleteCount",
                table: "Blocks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "UserFavoriteTeams");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "UserFavoriteTeams");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "UserFavoriteTeams");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "UserFavoriteTeams");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "UserFavoriteTeams");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "UserFavoriteTeams");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "UserFavoriteTeams");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "Tickets");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "Teams");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "Stadiums");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "RefreshTokens");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "Matches");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "CartItems");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "Cart");

            migrationBuilder.DropColumn(
                name: "BaseEntityStatus",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "CreatedBy",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ModifiedBy",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "Blocks");

            migrationBuilder.DropColumn(
                name: "SoftDeleteCount",
                table: "Blocks");

            migrationBuilder.AddForeignKey(
                name: "FK_RefreshTokens_BaseEntity_Id",
                table: "RefreshTokens",
                column: "Id",
                principalTable: "BaseEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserFavoriteTeams_BaseEntity_Id",
                table: "UserFavoriteTeams",
                column: "Id",
                principalTable: "BaseEntity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
