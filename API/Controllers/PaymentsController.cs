using Application.Payments.Service;
using Microsoft.AspNetCore.Mvc;
using Shared.DTOs.Payments;

namespace API.Controllers
{
    public class PaymentsController : BaseController
    {
        private readonly IPaymentService _paymentCallbackService;

        public PaymentsController(IPaymentService paymentCallbackService)
        {
            _paymentCallbackService = paymentCallbackService;
        }

        [HttpPost("callback")]
        public async Task<IActionResult> CallbackAsync([FromForm] TelrPaymentCallbackDto dto)
            => Result(await _paymentCallbackService.ProcessPaymentCallbackAsync(dto));
    }
}
