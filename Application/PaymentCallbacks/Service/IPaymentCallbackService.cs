using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.PaymentDTOs;

namespace Application.Payments.Service
{
    public interface IPaymentCallbackService
    {
        Task<BaseResponse<Empty>> ProcessPaymentCallbackAsync(PaymentCalllbackDto dto, CancellationToken cancellationToken = default);
    }
}
