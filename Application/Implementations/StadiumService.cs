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
    public class StadiumService : BaseService<Stadium, StadiumDto>, IStadiumService
    {
        public StadiumService(IUnitOfWork unitOfWork, ILogger<Stadium> logger, IMemoryCache cache, IMapper mapper, IValidator<StadiumDto> validator, IStringLocalizer<Resource> localizer) : base(unitOfWork, logger, cache, mapper, validator, localizer) { }

    }
}
