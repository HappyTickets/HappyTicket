using AutoMapper;
using Domain.Entities;
using Shared.DTOs;
using Shared.DTOs.Champion;
using Shared.DTOs.Stadium;
using Shared.DTOs.StadiumDTO;
using Shared.DTOs.Test.Request;
using Shared.DTOs.Test.Response;

namespace Application.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {

            CreateMap<Match, GetMatchDto>()
                       .ForMember(dest => dest.StadiumName, opt => opt.MapFrom(src => src.Stadium.Name))
                       .ForMember(dest => dest.ChampionName, opt => opt.MapFrom(src => src.Champion.Name))
                       .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));

            CreateMap<Match, CreateTestMatchDto>().ReverseMap();
            CreateMap<Match, UpdateTestMatchDto>().ReverseMap();
            CreateMap<Stadium, StadiumDto>().ReverseMap();
            CreateMap<Championship, ChampionDto>().ReverseMap();
            CreateMap<Stadium, GetStadiumDto>().ReverseMap();
            CreateMap<Stadium, CreateStadiumDto>().ReverseMap();


            //CreateMap<Ticket, TicketDto>().ReverseMap();
            //CreateMap<MatchO, MatchCommandDto>().ReverseMap();
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
            //CreateMap<ChampionO, ChampionDto>().ReverseMap();
            //CreateMap<CreateOrUpdateChampionDto, ChampionO>();
            ////.ForMember(dest => dest.ChampionSponsors, opt => opt.MapFrom(src => src.SponsorsIds.Select(id => new ChampionSponsor { SponsorId = id })));
            //CreateMap<ChampionSponsorO, ChampionSponsorDto>().ReverseMap();
            //CreateMap<TeamSponsorO, TeamSponsorDto>().ReverseMap();
        }
    }
}
