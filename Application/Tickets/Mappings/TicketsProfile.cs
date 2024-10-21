using AutoMapper;
using Domain.Entities;
using Shared.DTOs.TicketDTOs;
using Shared.Enums;

namespace Application.Tickets.Mappings
{
    internal class TicketsProfile : Profile
    {
        public TicketsProfile()
        {
            CreateMap<CreateTicketsDto, Ticket>()
                .ForMember(dest=>dest.TicketStatus, opt=>opt.MapFrom(src=>src.TicketStatus ?? TicketStatusDTO.Active));
            
            CreateMap<UpdateTicketsDto, Ticket>();
            CreateMap<Ticket, TicketDto>().ReverseMap();
        }
    }
}
