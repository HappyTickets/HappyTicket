using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.PaymentDTOs;

namespace Application.Payments.Service
{
    internal class PaymentCallbackService: IPaymentCallbackService
    {
        private readonly IUnitOfWork _ufw;

        public PaymentCallbackService(IUnitOfWork ufw)
        {
            _ufw = ufw;
        }

        public async Task<BaseResponse<Empty>> ProcessPaymentCallbackAsync(PaymentCalllbackDto dto, CancellationToken cancellationToken = default)
        {
            return Empty.Default;
        }
    }
}
