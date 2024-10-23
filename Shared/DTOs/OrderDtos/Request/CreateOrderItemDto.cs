using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.OrderDtos.Request
{
    public class CreateOrderItemDto
    {
        [Required]
        public long OrderId { get; set; }
        [Required]
        public long TicketId { get; set; }
        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }
    }


}
