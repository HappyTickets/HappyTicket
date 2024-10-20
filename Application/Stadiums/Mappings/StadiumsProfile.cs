using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Stadium;
using Shared.DTOs.StadiumDTO;
using Shared.DTOs;

namespace Application.Stadiums.Mappings
{
    internal class StadiumsProfile: Profile
    {
        public StadiumsProfile()
        {
            CreateMap<Stadium, StadiumDto>().ReverseMap();
            CreateMap<Stadium, GetStadiumDto>().ReverseMap();
            CreateMap<Stadium, CreateStadiumDto>().ReverseMap();
            CreateMap<Stadium, UpdateStadiumDto>().ReverseMap();
        }
    }
}
