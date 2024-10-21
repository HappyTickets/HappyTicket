using AutoMapper;
using Domain.Entities;
using Shared.DTOs.CartDTOs;

namespace Application.Carts.Mappings
{
    internal class CartsProfile: Profile
    {
        public CartsProfile()
        {
            CreateMap<Cart, CartDto>();
            CreateMap<CartItem, CartItemDto>()
                .ForMember(dest => dest.TicketTeam, opt => opt.MapFrom(src => src.Ticket.MatchTeam.Team.Name))
                .ForMember(dest => dest.HomeTeam, opt => opt.MapFrom(src => src.Ticket.MatchTeam.Match.MatchTeams.FirstOrDefault(mt => mt.IsHomeTeam).Team.Name))
                .ForMember(dest => dest.AwayTeam, opt => opt.MapFrom(src => src.Ticket.MatchTeam.Match.MatchTeams.FirstOrDefault(mt => !mt.IsHomeTeam).Team.Name))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Ticket.Class))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Ticket.Location))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.Ticket.Price))
                .ForMember(dest => dest.TotalPrice, opt => opt.MapFrom(src => src.Ticket.Price));
        }
    }
}
