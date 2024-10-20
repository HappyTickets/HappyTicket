﻿using FluentValidation;
using Shared.DTOs.TicketDTOs;

namespace Application.Validation.Ticket
{
    public class CreateTicketsDtoValidator: AbstractValidator<CreateTicketsDto>
    {
        public CreateTicketsDtoValidator()
        {
            RuleFor(dto => dto.MatchTeamId)
                .NotEmpty();
            
            RuleFor(dto => dto.Price)
                .NotEmpty()
                .GreaterThan(0)
                .PrecisionScale(18, 2, true);
            
            //RuleFor(dto => dto.BlockId)
            //    .NotEmpty();
            
            //RuleFor(dto => dto.SeatId)
            //    .NotEmpty();
            
            RuleFor(dto => dto.Location)
                .NotEmpty();
            
            RuleFor(dto => dto.Class)
                .NotEmpty();
            
            RuleFor(dto => dto.TicketStatus)
                .IsInEnum();
            
            //RuleFor(dto => dto.SeatNumber)
            //    .NotEmpty()
            //    .GreaterThan(0);
            
            RuleFor(dto => dto.InternalGate)
                .NotEmpty();
            
            RuleFor(dto => dto.ExternalGate)
                .NotEmpty();
           
            RuleFor(dto => dto.TicketsCount)
                .NotEmpty()
                .GreaterThan(0);
        }
    }
}
