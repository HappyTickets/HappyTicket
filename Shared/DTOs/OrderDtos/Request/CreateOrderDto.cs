using System.ComponentModel.DataAnnotations;

namespace Shared.DTOs.OrderDtos.Request
{
    public class CreateOrderDto
    {

        [Required]
        [Range(0, double.MaxValue)]
        public decimal TotalAmount { get; set; }

        public int? PaymentStatus { get; set; }

        public string? PaymentOrderRef { get; set; }

        public string? PaymentUrl { get; set; }

        [Required]
        public List<CreateOrderItemDto> OrderItems { get; set; } = new List<CreateOrderItemDto>();
    }

}
