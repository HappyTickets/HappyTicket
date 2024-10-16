//using Application.Interfaces.PaymentServices;
//using Microsoft.AspNetCore.Mvc;
//using Shared.DTOs.PaymentDTOs;

//namespace API.Controllers
//{
//    public class PaymentsController : BaseController
//    {
//        private readonly IPaymentService _paymentService;


//        public PaymentsController(IHttpContextAccessor httpContextAccessor, IPaymentService paymentService) : base(httpContextAccessor)
//        {
//            _paymentService = paymentService;
//        }


//        [HttpPost("Request")]
//        public async Task<ActionResult> SendPaymentRequest([FromBody] PaymentRequestDto paymentRequestDto)
//        {
//            try
//            {
//                return ReturnResult(await _paymentService.SendPaymentRequestAsync(paymentRequestDto));
//            }
//            catch (Exception ex)
//            {
//                return ReturnException(ex);
//            }
//        }

//        //[HttpGet("status/{orderId}")]
//        //public async Task<IActionResult> CheckPaymentStatus(Guid orderId)
//        //{
//        //    var response = await _paymentService.CheckPaymentStatusAsync(orderId);
//        //    if (response.HasErrors)
//        //    {
//        //        return BadRequest(response.Errors);
//        //    }
//        //    return Ok(response);
//        //}
//    }
//}
