using AutoMapper;
using Domain.Entities;
using Shared.DTOs.MatchDtos;
using Shared.DTOs.Test.Request;
using Shared.DTOs.Test.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Application.Matches.Mappings
{
    internal class MatchesProfile : Profile
    {
        public MatchesProfile()
        {
            CreateMap<Domain.Entities.Match, CreateMatchDto>().ReverseMap();
            CreateMap<Domain.Entities.Match, UpdateMatchDto>().ReverseMap();
            CreateMap<Domain.Entities.Match, GetPaginatedMatchesDto>().ReverseMap();
            CreateMap<Domain.Entities.Match, FindActiveMatchesDto>().ReverseMap();
            CreateMap<Domain.Entities.Match, GetMatchByIdDto>()
                       .ForMember(dest => dest.StadiumName, opt => opt.MapFrom(src => src.Stadium.Name))
                       .ForMember(dest => dest.ChampionName, opt => opt.MapFrom(src => src.Champion.Name)).ReverseMap();

            CreateMap<Domain.Entities.Match, GetAllMatchesDto>()
                       .ForMember(dest => dest.StadiumName, opt => opt.MapFrom(src => src.Stadium.Name))
                       .ForMember(dest => dest.ChampionName, opt => opt.MapFrom(src => src.Champion.Name)).ReverseMap();
        }
    }
}
