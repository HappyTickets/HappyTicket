using AutoMapper;
using Domain.Entities;
using Shared.DTOs.Sponsors;

namespace Application.Sponsors.Mappings
{
    public class SponsorsProfile: Profile
    {
        public SponsorsProfile()
        {
            CreateMap<Sponsor, SponsorDto>();
            CreateMap<CreateOrUpdateSponsorDto, Sponsor>();
        }
    }
}
