using AutoMapper;
using Domain.Entities;
using Domain.Entities.CartEntity;
using Domain.Entities.UserEntities;
using Domain.Entities.UserEntities.AuthEntities;
using Shared.DTOs;
using Shared.DTOs.Authorization.Response;
using Shared.DTOs.CartDTOs;
using Shared.DTOs.Identity.UserDTOs;
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
                .ForMember(x => x.TeamId, opt => opt.MapFrom(x => x.TeamId))
                .ReverseMap();
            CreateMap<Order, OrderDto>().ReverseMap();

            CreateMap<ApplicationUser, ApplicationUserDTO>()
                .ForMember(u => u.Id, opt => opt.MapFrom(r => r.Id))
                .ForMember(u => u.Email, opt => opt.MapFrom(r => r.Email))
                .ForMember(u => u.PhoneNumber, opt => opt.MapFrom(r => r.PhoneNumber))
                .ForMember(u => u.UserName, opt => opt.MapFrom(r => r.UserName)).ReverseMap();


            CreateMap<Role, RoleDto>()
            .ForMember(r => r.RoleId, opt => opt.MapFrom(r => r.Id))
            .ForMember(r => r.RoleName, opt => opt.MapFrom(r => r.Name))
            .ForMember(r => r.RoleDescription, opt => opt.MapFrom(r => r.Description))
            .ReverseMap();

            //CreateMap<Block, BlockDTO>().ForMember(x => x.TicketsDTO, opt => opt.MapFrom(x => x.Tickets)).ReverseMap();
        }
    }
}
