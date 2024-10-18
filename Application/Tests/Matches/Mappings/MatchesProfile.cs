using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Test.Request;
using Shared.DTOs.Test.Response;

namespace Application.Tests.Matches.Mappings
{
    internal class MatchesProfile: Profile
    {
        public MatchesProfile()
        {
            CreateMap<Match, CreateTestMatchDto>().ReverseMap();
            CreateMap<Match, UpdateTestMatchDto>().ReverseMap();

            CreateMap<Match, GetMatchDto>()
                       .ForMember(dest => dest.StadiumName, opt => opt.MapFrom(src => src.Stadium.Name))
                       .ForMember(dest => dest.ChampionName, opt => opt.MapFrom(src => src.Champion.Name))
                       .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive));
        }
    }
}
