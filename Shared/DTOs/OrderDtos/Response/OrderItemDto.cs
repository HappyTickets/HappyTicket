using Shared.DTOs.TicketDTOs;

namespace Shared.DTOs.OrderDtos.Response
{
    public class OrderItemDto
    {
        public long Id { get; set; }
        public long OrderId { get; set; }
        public long TicketId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public TicketDto TicketDto { get; set; } = new TicketDto();

    }

}
