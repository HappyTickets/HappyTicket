using Domain.Entities.CartEntity;
using LanguageExt;
using LanguageExt.Common;
using Shared.DTOs.PaymentDTOs;

namespace Application.Interfaces.PaymentServices
{
    public interface IPaymentService
    {
        Task<Result<PaymentResponseDto>> SendPaymentRequestAsync(PaymentRequestDto paymentRequestDto);
        Task<PaymentStatusDto> CheckPaymentStatusAsync(Guid orderId);
        Task<Result<Unit>> SendTicketEmailAsync(string email, ICollection<CartItemO> ticketItems, CancellationToken cancellationToken = default);
    }
}
