using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.Common.General;
using Shared.DTOs.OrderDtos.Request;
using Shared.DTOs.OrderDtos.Response;
using Shared.Exceptions;
using Shared.ResourceFiles;

namespace Application.Orders.Service
{

    public class OrderService(
          IUnitOfWork unitOfWork,
          ILogger<Order> logger,
          IMapper mapper,
          ICurrentUser currentUser
          ) : BaseService<Order>(unitOfWork, logger, mapper), IOrderService

    {
        private readonly ICurrentUser _currentUser = currentUser;

        public async ValueTask<BaseResponse<object?>> CreateOrderAsync(CreateOrderDto dto, CancellationToken cancellationToken = default)
        {
            await CreateAsync(dto, true, cancellationToken);
            return new();
        }

        public async ValueTask<BaseResponse<OrderDto?>> GetOrderByIdAsync(long id, CancellationToken cancellationToken = default)
        {
            var includes = new List<string>()
            {
                nameof(Order.User),
                nameof(Order.OrderItems),
                nameof(Order.OrderItems) + '.' + nameof(OrderItem.Ticket)
            };

            var order = await GetByIdAsync<OrderDto>(id, cancellationToken, includes);
            return new BaseResponse<OrderDto?>(order);
        }

        public async ValueTask<BaseResponse<object?>> UpdateOrderAsync(UpdateOrderDto dto, CancellationToken cancellationToken = default)
        {
            var order = await GetByIdAsync<Order>(dto.Id, cancellationToken);

            if (order is null)
                return new NotFoundException(Resource.NotFoundInDB_Message);

            if (order.UserId != _currentUser.Id)
                return new NotAuthorizedException();

            _mapper.Map(dto, order);

            await UpdateAsync(order, true, cancellationToken);

            return new();
        }


        public async ValueTask<BaseResponse<object?>> SoftDeleteOrderAsync(long id, CancellationToken cancellationToken = default)
        {
            await SoftDeleteByIdAsync(id, true, cancellationToken);
            return new();
        }

        public async ValueTask<BaseResponse<object?>> HardDeleteOrderAsync(long id, CancellationToken cancellationToken = default)
        {

            await HardDeleteByIdAsync(id, true, cancellationToken);
            return new();
        }

        public async ValueTask<BaseResponse<PaginatedList<OrderDto>>> GetAllOrdersAsync(PaginationSearchModel paginationSearchModel, CancellationToken cancellationToken = default)
        {
            var includes = new List<string>()
            {
                nameof(Order.User),
                nameof(Order.OrderItems),
            };

            var ordersList = await _unitOfWork.Orders.GetAllOrdersAsync(paginationSearchModel, includes, cancellationToken);
            var ordersDtos = _mapper.Map<IEnumerable<OrderDto>>(ordersList.Items);

            return new PaginatedList<OrderDto>(ordersDtos, ordersList.TotalItems, paginationSearchModel.PageIndex, paginationSearchModel.PageSize);
        }
    }

}
