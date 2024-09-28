namespace Shared.DTOs
{
    public class OrderItemDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Guid TicketId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
