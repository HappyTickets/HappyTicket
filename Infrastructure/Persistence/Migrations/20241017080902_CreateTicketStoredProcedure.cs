using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class CreateTicketStoredProcedure : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var storedProcedure = "CREATE PROCEDURE CreateTickets (\r\n\t@matchTeamId bigint,\r\n\t@price decimal(18, 2),\r\n\t@notes nvarchar(max),\r\n\t@blockId bigint,\r\n\t@seatId bigint,\r\n\t@displayForSale bit,\r\n\t@location nvarchar(max),\r\n\t@class nvarchar(max),\r\n\t@ticketStatus int,\r\n\t@seatNumber int,\r\n\t@externalGate nvarchar(max),\r\n\t@internalGate nvarchar(max),\r\n\t@baseEntityStatus int,\r\n\t@createdBy bigint,\r\n\t@createdDate dateTime2(7),\r\n\t@modifiedBy bigint,\r\n\t@modifiedDate datetime2(7),\r\n\t@isActive bit,\r\n\t@softDeleteCount int,\r\n\t@ticketsCount int\r\n)\r\nAS\r\nBEGIN\r\n\tSET NOCOUNT ON;\r\n\r\n\tDECLARE @TicketsToInsert TABLE (\r\n\t\tMatchTeamId bigint,\r\n\t\tPrice decimal(18, 2),\r\n\t\tNotes nvarchar(max),\r\n\t\tBlockId bigint,\r\n\t\tSeatId bigint,\r\n\t\tDisplayForSale bit,\r\n\t\tLocation nvarchar(max),\r\n\t\tClass nvarchar(max),\r\n\t\tTicketStatus int,\r\n\t\tSeatNumber int,\r\n\t\tExternalGate nvarchar(max),\r\n\t\tInternalGate nvarchar(max),\r\n\t\tBaseEntityStatus int,\r\n\t\tCreatedBy bigint,\r\n\t\tCreatedDate datetime2,\r\n\t\tModifiedBy bigint,\r\n\t\tModifiedDate datetime2,\r\n\t\tIsActive bit,\r\n\t\tSoftDeleteCount int\r\n\t);\r\n\r\n\tDECLARE @count int = 0;\r\n\tWHILE @count < @ticketsCount\r\n\tBEGIN\r\n\t\tINSERT INTO @TicketsToInsert (MatchTeamId, Price, Notes, BlockId, SeatId, DisplayForSale, Location, Class, TicketStatus, SeatNumber, ExternalGate, InternalGate, BaseEntityStatus, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, SoftDeleteCount) \r\n\t\t\t\t\t\t\t VALUES (@matchTeamId, @price, @notes, @blockId, @seatId, @displayForSale, @location, @class, @ticketStatus, @seatNumber, @externalGate, @internalGate, @baseEntityStatus, @createdBy, @createdDate, @modifiedBy, @modifiedDate, @isActive, @softDeleteCount)\r\n\t\tSET @count = @count + 1\r\n\tEND\r\n\r\n\tINSERT INTO [dbo].[Tickets] (MatchTeamId, Price, Notes, BlockId, SeatId, DisplayForSale, Location, Class, TicketStatus, SeatNumber, ExternalGate, InternalGate, BaseEntityStatus, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, SoftDeleteCount)\r\n\tSELECT MatchTeamId, Price, Notes, BlockId, SeatId, DisplayForSale, Location, Class, TicketStatus, SeatNumber, ExternalGate, InternalGate, BaseEntityStatus, CreatedBy, CreatedDate, ModifiedBy, ModifiedDate, IsActive, SoftDeleteCount \r\n\tFROM @TicketsToInsert\r\nEND";
            migrationBuilder.Sql(storedProcedure);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE CreateTickets");
        }
    }
}
