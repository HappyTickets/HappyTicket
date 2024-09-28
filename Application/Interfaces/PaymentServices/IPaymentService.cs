using Domain.Entities.CartEntity;
using LanguageExt.Common;
using LanguageExt;
using Shared.DTOs.PaymentDTOs;

namespace Application.Interfaces.PaymentServices
{
    public interface IPaymentService
    {
        Task<Result<PaymentResponseDto>> SendPaymentRequestAsync(PaymentRequestDto paymentRequestDto);
        Task<PaymentStatusDto> CheckPaymentStatusAsync(Guid orderId);
        Task<Result<Unit>> SendTicketEmailAsync(string email, ICollection<CartItem> ticketItems, CancellationToken cancellationToken = default);
    }
}
