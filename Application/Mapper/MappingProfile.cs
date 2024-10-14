using AutoMapper;
using Domain.Entities;
using Domain.Entities.CartEntity;
using Domain.Entities.UserEntities;
using Shared.DTOs;
using Shared.DTOs.CartDTOs;
using Shared.DTOs.Champion;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.Team;
using Shared.DTOs.TicketDTOs;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Ticket, TicketDto>().ReverseMap();
            CreateMap<MatchO, MatchDto>()
               .ForMember(x => x.TeamA, opt => opt.MapFrom(x => x.TeamA))
               .ForMember(x => x.TeamB, opt => opt.MapFrom(x => x.TeamB))
               .ForMember(x => x.Stadium, opt => opt.MapFrom(x => x.Stadium))
               .ReverseMap();
            CreateMap<MatchO, MatchCommandDto>().ReverseMap();
            CreateMap<StadiumO, StadiumDto>().ReverseMap();
            CreateMap<Team, TeamDto>().ReverseMap();
            CreateMap<CreateOrUpdateTeamDto ,Team>();
            CreateMap<SeatO, SeatDto>().ReverseMap();
            CreateMap<BlockO, BlockDto>().ReverseMap();
            CreateMap<Cart, CartDto>().ReverseMap();
            CreateMap<CartItem, CartItemDto>().ReverseMap();
            CreateMap<SelectedTeam, UserFavoriteTeamDto>()
                .ForMember(x => x.TeamId, opt => opt.MapFrom(x => x.TeamId))
                .ReverseMap();
            CreateMap<OrderO, OrderDto>().ReverseMap();
            //CreateMap<Block, BlockDTO>().ForMember(x => x.TicketsDTO, opt => opt.MapFrom(x => x.Tickets)).ReverseMap();
            CreateMap<SponsorO, SponsorDto>().ReverseMap();
            CreateMap<ChampionO, ChampionDto>().ReverseMap();
            CreateMap<CreateOrUpdateChampionDto, ChampionO>();
                //.ForMember(dest => dest.ChampionSponsors, opt => opt.MapFrom(src => src.SponsorsIds.Select(id => new ChampionSponsor { SponsorId = id })));
            CreateMap<ChampionSponsorO, ChampionSponsorDto>().ReverseMap();
            CreateMap<TeamSponsorO, TeamSponsorDto>().ReverseMap();
        }
    }
}
