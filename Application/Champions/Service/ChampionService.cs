using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.DTOs.Champion;

namespace Application.Implementations
{
    public class ChampionService : BaseService<Championship>, IChampionService
    {
        public ChampionService(IUnitOfWork unitOfWork, ILogger<Championship> logger, IMapper mapper) : base(unitOfWork, logger, mapper) { }

        public ValueTask<BaseResponse<ChampionDto>> CreateAsync(CreateOrUpdateChampionDto dto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public ValueTask<BaseResponse<ChampionDto>> UpdateAsync(CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
