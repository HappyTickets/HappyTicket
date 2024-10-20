using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Champion;

namespace Application.Champions.Mappings
{
    internal class ChampionsProfile : Profile
    {
        public ChampionsProfile()
        {
            CreateMap<Championship, CreateChampionshipDto>().ReverseMap();
        }
    }
}
