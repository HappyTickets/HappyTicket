using AutoMapper;
using Domain.Entities;
using Domain.Entities.UserEntities;
using Shared.DTOs.OrderDtos.Request;
using Shared.DTOs.OrderDtos.Response;

namespace Application.Orders.Mappings
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            // Mapping for Order
            CreateMap<Order, CreateOrderDto>().ReverseMap();
            CreateMap<Order, UpdateOrderDto>().ReverseMap();
            CreateMap<Order, OrderDto>()
                .ForMember(oi => oi.Owner, x => x.MapFrom(src => src.User))
                .ReverseMap();

            // Mapping for OrderItem
            CreateMap<OrderItem, CreateOrderItemDto>();
            CreateMap<OrderItem, UpdateOrderItemDto>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(oi => oi.TicketDto, x => x.MapFrom(src => src.Ticket))
                .ReverseMap();

            // orderOwner 
            CreateMap<ApplicationUser, OrderOwner>();

        }
    }

}
