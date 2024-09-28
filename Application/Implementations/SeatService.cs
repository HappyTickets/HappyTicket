using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shared.DTOs;
using Shared.ResourceFiles;

namespace Application.Implementations
{
    public class SeatService : BaseService<Seat, SeatDto>, ISeatService
    {
        public SeatService(IUnitOfWork unitOfWork, ILogger<Seat> logger, IMemoryCache cache, IMapper mapper, IValidator<SeatDto> validator, IStringLocalizer<Resource> localizer) : base(unitOfWork, logger, cache, mapper, validator, localizer) { }

    }
}
