using AutoMapper;
using Domain.Entities;
using Shared.DTOs;
using Shared.DTOs.MatchDtos;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Match, MatchDto>()
               .ReverseMap();

            CreateMap<MatchCreateOrUpdateDto, Match>()
          .ForMember(dest => dest.Id, opt => opt.Condition(src => src.Id > 0));

            //CreateMap<Ticket, TicketDto>().ReverseMap();
            //CreateMap<MatchO, MatchCommandDto>().ReverseMap();
            CreateMap<Stadium, StadiumDto>().ReverseMap();
            //CreateMap<Team, TeamDto>().ReverseMap();
            //CreateMap<CreateOrUpdateTeamDto, Team>();
            //CreateMap<SeatO, SeatDto>().ReverseMap();
            //CreateMap<BlockO, BlockDto>().ReverseMap();
            //CreateMap<Cart, CartDto>().ReverseMap();
            //CreateMap<CartItem, CartItemDto>().ReverseMap();
            //CreateMap<SelectedTeam, UserFavoriteTeamDto>()
            //    .ForMember(x => x.TeamId, opt => opt.MapFrom(x => x.TeamId))
            //    .ReverseMap();
            //CreateMap<OrderO, OrderDto>().ReverseMap();
            ////CreateMap<Block, BlockDTO>().ForMember(x => x.TicketsDTO, opt => opt.MapFrom(x => x.Tickets)).ReverseMap();
            //CreateMap<SponsorO, SponsorDto>().ReverseMap();
            //CreateMap<Championship, ChampionshipDto>().ReverseMap();
            //CreateMap<CreateOrUpdateChampionDto, ChampionO>();
            ////.ForMember(dest => dest.ChampionSponsors, opt => opt.MapFrom(src => src.SponsorsIds.Select(id => new ChampionSponsor { SponsorId = id })));
            //CreateMap<ChampionSponsorO, ChampionSponsorDto>().ReverseMap();
            //CreateMap<TeamSponsorO, TeamSponsorDto>().ReverseMap();
        }
    }
}
