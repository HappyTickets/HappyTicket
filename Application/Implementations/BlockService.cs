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
    public class BlockService : BaseService<Block, BlockDto>, IBlockService
    {
        public BlockService(IUnitOfWork unitOfWork, ILogger<Block> logger, IMemoryCache cache, IMapper mapper, IValidator<BlockDto> validator, IStringLocalizer<Resource> localizer) : base(unitOfWork, logger, cache, mapper, validator, localizer) { }

    }
}
