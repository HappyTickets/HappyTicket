using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations
{
    internal class CartItemConfig : IEntityTypeConfiguration<CartItem>
    {
        public void Configure(EntityTypeBuilder<CartItem> builder)
        {
            builder.HasOne(ci => ci.Ticket)
                   .WithMany()
                   .HasForeignKey(ci => ci.TicketId);
        }
    }
}
