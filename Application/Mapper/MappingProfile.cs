using AutoMapper;
using Domain.Entities;
using Domain.Entities.CartEntity;
using Domain.Entities.UserEntities;
using Shared.DTOs;
using Shared.DTOs.CartDTOs;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.TicketDTOs;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<Match, MatchDto>()
               .ForMember(x => x.TeamA, opt => opt.MapFrom(x => x.TeamA))
               .ForMember(x => x.TeamB, opt => opt.MapFrom(x => x.TeamB))
               .ForMember(x => x.Stadium, opt => opt.MapFrom(x => x.Stadium))
               .ReverseMap();
            CreateMap<Match, MatchCommandDto>().ReverseMap();
            CreateMap<Stadium, StadiumDto>().ReverseMap();
            CreateMap<Team, TeamDto>().ReverseMap();
            CreateMap<Seat, SeatDto>().ReverseMap();
            CreateMap<Block, BlockDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<UserFavoriteTeam, UserFavoriteTeamDto>()
                .ForMember(x=>x.TeamId,opt=>opt.MapFrom(x=>x.TeamId))
                .ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();
            //CreateMap<Block, BlockDTO>().ForMember(x => x.TicketsDTO, opt => opt.MapFrom(x => x.Tickets)).ReverseMap();
        }
    }
}
