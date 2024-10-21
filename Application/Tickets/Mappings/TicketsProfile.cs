using AutoMapper;
using Domain.Entities;
using Shared.DTOs.TicketDTOs;

namespace Application.Tickets.Mappings
{
    internal class TicketsProfile : Profile
    {
        public TicketsProfile()
        {
            CreateMap<CreateTicketsDto, Ticket>();
            CreateMap<UpdateTicketsDto, Ticket>();
            CreateMap<Ticket, TicketDto>().ReverseMap();
        }
    }
}
