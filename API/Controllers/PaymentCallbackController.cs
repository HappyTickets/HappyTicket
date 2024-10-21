using Application.Payments.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.PaymentDTOs;

namespace API.Controllers
{
    public class PaymentCallbackController : BaseController
    {
        private readonly IPaymentCallbackService _paymentCallbackService;

        public PaymentCallbackController(IPaymentCallbackService paymentCallbackService)
        {
            _paymentCallbackService = paymentCallbackService;
        }

        [HttpPost("callback")]
        public async Task<IActionResult> CallbackAsync(PaymentCalllbackDto dto, CancellationToken cancellationToken)
            => Result(await _paymentCallbackService.ProcessPaymentCallbackAsync(dto, cancellationToken));
    }
}
