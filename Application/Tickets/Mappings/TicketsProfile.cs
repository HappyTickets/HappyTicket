using AutoMapper;
using Domain.Entities;
using Shared.Common.General;
using Shared.DTOs.TicketDTOs;
using Shared.DTOs.Tickets;
using Shared.Enums;
using static Shared.DTOs.Tickets.RichTicketDto;

namespace Application.Tickets.Mappings
{
    internal class TicketsProfile : Profile
    {
        public TicketsProfile()
        {
            CreateMap<CreateTicketsDto, Ticket>()
                .ForMember(dest => dest.TicketStatus, opt => opt.MapFrom(src => src.TicketStatus ?? TicketStatusDTO.Active));

            CreateMap<UpdateTicketsDto, Ticket>();
            CreateMap<Ticket, TicketDto>().ReverseMap();

            CreateMap<Ticket, RichTicketDto>()
            .ForMember(dest => dest.TicketId, opt => opt.MapFrom(src => LongIdEncryptionHelper.EncryptId(src.Id)))
            .ForMember(dest => dest.TicketStatus, opt => opt.MapFrom(src => src.TicketStatus.ToString()))
            .ForMember(dest => dest.Stadium, opt => opt.MapFrom(src => src.MatchTeam.Match.Stadium))
            .ForMember(dest => dest.SelectedTeam, opt => opt.MapFrom(src => src.MatchTeam))
            .ForMember(dest => dest.HomeTeam, opt => opt.MapFrom(src =>
                  new TeamDto
                  {
                      TeamId = src.MatchTeam.Match.MatchTeams.FirstOrDefault(mt => mt.IsHomeTeam).TeamId,
                      TeamName = src.MatchTeam.Match.MatchTeams.FirstOrDefault(mt => mt.IsHomeTeam).Team.Name,
                      TeamSponsors = src.MatchTeam.Match.MatchTeams.FirstOrDefault(mt => mt.IsHomeTeam).Team.TeamSponsors
                          .Select(ts => new SponsorDto
                          {
                              SponsorId = ts.SponsorId,
                              Name = ts.Sponsor.Name,
                              Logo = ts.Sponsor.Logo
                          }).ToList()
                  }))
              .ForMember(dest => dest.AwayTeam, opt => opt.MapFrom(src =>
                  new TeamDto
                  {
                      TeamId = src.MatchTeam.Match.MatchTeams.FirstOrDefault(mt => !mt.IsHomeTeam).TeamId,
                      TeamName = src.MatchTeam.Match.MatchTeams.FirstOrDefault(mt => !mt.IsHomeTeam).Team.Name,
                      TeamSponsors = src.MatchTeam.Match.MatchTeams.FirstOrDefault(mt => !mt.IsHomeTeam).Team.TeamSponsors
                          .Select(ts => new SponsorDto
                          {
                              SponsorId = ts.SponsorId,
                              Name = ts.Sponsor.Name,
                              Logo = ts.Sponsor.Logo
                          }).ToList()
                  }))

            .ForMember(dest => dest.Champion, opt => opt.MapFrom(src => src.MatchTeam.Match.Champion))
          .ReverseMap();

            CreateMap<Stadium, StadiumDto>();
            CreateMap<Championship, ChampionDto>()
            .ForMember(dest => dest.ChampionSponsors, opt => opt.MapFrom(src => src.ChampionSponsors
            .Select(cs => new SponsorDto
            {
                SponsorId = cs.SponsorId,
                Name = cs.Sponsor.Name,
                Logo = cs.Sponsor.Logo
            }).ToList()));

            CreateMap<MatchTeam, MatchTeamDto>()
            .ForMember(dest => dest.MatchTeamName, opt => opt.MapFrom(src => src.Team.Name));

        }
    }
}
