using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal class TicketConfig : IEntityTypeConfiguration<Ticket>
    {
        public void Configure(EntityTypeBuilder<Ticket> builder)
        {
            builder.HasOne(t => t.Block)
                .WithMany()
                .HasForeignKey(t => t.BlockId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t=>t.Seat)
                .WithMany()
                .HasForeignKey(t=>t.SeatId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(t => t.CartItems)
                .WithOne(ci => ci.Ticket)
                .HasForeignKey(ci => ci.TicketId);
        }
    }
}
