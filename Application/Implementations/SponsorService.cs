﻿using Application.Interfaces;
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
    public class SponsorService : BaseService<Sponsor, SponsorDto>, ISponsorService
    {
        public SponsorService(IUnitOfWork unitOfWork, ILogger<Sponsor> logger, IMemoryCache cache, IMapper mapper, IValidator<SponsorDto> validator, IStringLocalizer<Resource> localizer) : base(unitOfWork, logger, cache, mapper, validator, localizer)
        {
        }
    }
}
