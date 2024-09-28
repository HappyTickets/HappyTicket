using Shared.DTOs.TicketDTOs;

namespace Shared.DTOs.CartDTOs
{
    public class CartItemDto
    {
        public Guid Id { get; set; }
        public Guid CartId { get; set; }
        public Guid TicketId { get; set; }
        public TicketDto? Ticket { get; set; }
        public int Quantity { get; set; }
        public bool IsCheckedOut { get; set; }
    }
}
