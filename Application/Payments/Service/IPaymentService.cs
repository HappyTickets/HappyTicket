using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.Payments;

namespace Application.Payments.Service
{
    public interface IPaymentService
    {
        Task<BaseResponse<Empty>> ProcessPaymentCallbackAsync(TelrPaymentCallbackDto dto);
    }
}
