using Application.Interfaces;
using LanguageExt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs;
using Shared.DTOs.TicketDTOs;
using Shared.Extensions;
using System.Net;

namespace API.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        private readonly IOrderService _orderService;


        public OrderController(IHttpContextAccessor httpContextAccessor, IOrderService orderService) : base(httpContextAccessor)
        {
            _orderService = orderService;
        }

        [HttpGet]
        [Route("GetMyOrders")]
        public async Task<ActionResult> GetMyOrders(CancellationToken cancellationToken = default)
        {
            try
            {
                // Get current user's ID from claims
                var userId = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "Id")?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized("User not authenticated.");
                }

                // Call the service method to get user's orders
                var result = await _orderService.GetTicketsByUserIdAsync(userId);

                // Check if the result is successful
                if (result.TryGetValue(out var tickets, out var error))
                {
                    var response = new BaseListResponse<IEnumerable<TicketDto>>
                    {
                        Status = HttpStatusCode.OK,
                        Data = tickets, // Ensure this is IEnumerable<TicketDto>
                    };
                    return Ok(response);
                }
                else
                {
                    return StatusCode((int)HttpStatusCode.InternalServerError, error.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetOrders")]
        public async Task<ActionResult> GetOrders(bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _orderService.GetAllAsync(useCache, cancellationToken: cancellationToken);
                return ReturnListResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet]
        [Route("GetOrdersCount")]
        public async Task<ActionResult> GetOrdersCountAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                var result = await _orderService.GetLongCountAsync(cancellationToken);

                return ReturnResult(result);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("GetPaginatedOrders")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetPaginatedOrdersAsync([FromBody] PaginationSearchModel paginationParams, bool useCache = false, CancellationToken cancellationToken = default)
        {
            try
            {

                var result = await _orderService.GetPaginatedOrdersAsync(paginationParams, useCache, null, cancellationToken);
                return ReturnListResult(result);

            }
            catch (Exception ex)
            {

                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }

        }

        [HttpGet]
        [Route("GetOrder")]
        public async Task<ActionResult> GetOrderById(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _orderService.GetByIdAsync(id, useCache, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost]
        [Route("AddOrder")]
        public async Task<ActionResult> AddOrder([FromBody] OrderDto orderDto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _orderService.CreateAsync(orderDto, cancellationToken: cancellationToken);
                    return ReturnResult(result);
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [Route("EditOrder")]
        public async Task<ActionResult> EditOrder([FromBody] OrderDto orderDto, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _orderService.UpdateAsync(orderDto, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete]
        [Route("DeleteOrder")]
        public async Task<ActionResult> DeleteOrder(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    return ReturnResult(await _orderService.HardDeleteByIdAsync(id, cancellationToken: cancellationToken));
                }

                return BadRequest(ModelState);
            }
            catch (Exception ex)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
