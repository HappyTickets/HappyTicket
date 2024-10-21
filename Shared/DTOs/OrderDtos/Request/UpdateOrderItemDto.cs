using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.OrderDtos.Request
{
    public class UpdateOrderItemDto
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public long OrderId { get; set; }
        [Required]
        public long TicketId { get; set; }
        [Required]
        [Range(0.0, double.MaxValue)]
        public decimal Price { get; set; }
    }


}
