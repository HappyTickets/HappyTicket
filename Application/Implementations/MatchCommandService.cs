﻿using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shared.DTOs.MatchDtos;
using Shared.ResourceFiles;

namespace Application.Implementations
{
    public class MatchCommandService : BaseService<Match, MatchCommandDto>, IMatchCommandService
    {
        public MatchCommandService(IUnitOfWork unitOfWork, ILogger<Match> logger, IMemoryCache cache, IMapper mapper, IValidator<MatchCommandDto> validator, IStringLocalizer<Resource> localizer) : base(unitOfWork, logger, cache, mapper, validator, localizer) { }
    }
}
