using Application.Common.Interfaces.Services;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common.General;
using Shared.DTOs.OrderDtos.Request;

namespace API.Controllers
{
    [Authorize]
    public class OrderController(IOrderService orderService, ICurrentUser currentUser) : BaseController
    {
        private readonly IOrderService _orderService = orderService;
        private readonly ICurrentUser _currentUser = currentUser;

        //        [HttpGet]
        //        [Route("GetMyOrders")]
        //        public async Task<ActionResult> GetMyOrders(CancellationToken cancellationToken = default)
        //        {
        //            try
        //            {
        //                var userId = _currentUser.Id;
        //                var result = await _orderService.GetTicketsByUserIdAsync(userId);

        //                // Check if the result is successful
        //                if (result.TryGetValue(out var tickets, out var error))
        //                {
        //                    var response = new BaseListResponse<IEnumerable<TicketDto>>
        //                    {
        //                        Status = HttpStatusCode.OK,
        //                        Data = tickets, // Ensure this is IEnumerable<TicketDto>
        //                    };
        //                    return Ok(response);
        //                }
        //                else
        //                {
        //                    return StatusCode((int)HttpStatusCode.InternalServerError, error.Message);
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //            }
        //        }

        //        //[HttpGet]
        //        //[Route("GetOrders")]
        //        //public async Task<ActionResult> GetOrders(bool useCache = true, CancellationToken cancellationToken = default)
        //        //{
        //        //    try
        //        //    {
        //        //        var result = await _orderService.GetAllAsync(useCache, cancellationToken: cancellationToken);
        //        //        return ReturnListResult(result);
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //        //    }
        //        //}

        //        //[HttpGet]
        //        //[Route("GetOrdersCount")]
        //        //public async Task<ActionResult> GetOrdersCountAsync(CancellationToken cancellationToken = default)
        //        //{
        //        //    try
        //        //    {
        //        //        var result = await _orderService.GetLongCountAsync(cancellationToken);

        //        //        return ReturnResult(result);
        //        //    }
        //        //    catch (Exception ex)
        //        //    {
        //        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //        //    }
        //        //}

        //        //[HttpPost]
        //        //[Route("GetPaginatedOrders")]
        //        //public async Task<ActionResult<IEnumerable<OrderDto>>> GetPaginatedOrdersAsync([FromBody] PaginationSearchModel paginationParams, bool useCache = false, CancellationToken cancellationToken = default)
        //        //{
        //        //    try
        //        //    {

        //        //        var result = await _orderService.GetPaginatedOrdersAsync(paginationParams, useCache, null, cancellationToken);
        //        //        return ReturnListResult(result);

        //        //    }
        //        //    catch (Exception ex)
        //        //    {

        //        //        return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
        //        //    }

        //        //}

        [HttpGet]
        [Route("Get/{id}")]
        public async Task<IActionResult> GetOrderById(long id, CancellationToken cancellationToken = default)
    => Result(await _orderService.GetOrderByIdAsync(id, cancellationToken: cancellationToken));


        [HttpGet]
        [Route("GetAll")]
        // paginated only search not yet
        public async Task<IActionResult> GetAllOrders([FromQuery] PaginationParams pagination, CancellationToken cancellationToken = default)
         => Result(await _orderService.GetAllOrdersAsync(pagination, cancellationToken: cancellationToken));


        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> AddOrder([FromBody] CreateOrderDto orderDto, CancellationToken cancellationToken = default)
        {
            orderDto.UserId = _currentUser.Id ?? 0;

            return Result(await _orderService.CreateOrderAsync(orderDto, cancellationToken: cancellationToken));

        }


        [HttpPut]
        [Route("Update")]
        public async Task<IActionResult> EditOrder([FromBody] UpdateOrderDto orderDto, CancellationToken cancellationToken = default)
        {
            orderDto.UserId = _currentUser.Id ?? 0;
            return Result(await _orderService.UpdateOrderAsync(orderDto, cancellationToken: cancellationToken));
        }



        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> DeleteOrder(long id, CancellationToken cancellationToken = default)

                    => Result(await _orderService.HardDeleteOrderAsync(id, cancellationToken: cancellationToken));



    }
}
