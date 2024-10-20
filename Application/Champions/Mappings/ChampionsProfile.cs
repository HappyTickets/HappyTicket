using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Champion;
using Shared.DTOs.ChampionDtos;

namespace Application.Champions.Mappings
{
    internal class ChampionsProfile : Profile
    {
        public ChampionsProfile()
        {
            CreateMap<Championship, CreateChampionshipDto>().ReverseMap();
            CreateMap<Championship, UpdateChampionshipDto>().ReverseMap();
            CreateMap<Championship, GetChampionshipDto>().ReverseMap();
        }
    }
}
