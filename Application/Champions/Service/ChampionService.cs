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

        public async ValueTask<BaseResponse<CreateChampionshipDto>> CreateAsync(CreateChampionshipDto createChampionshipDto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var championship = _mapper.Map<Championship>(createChampionshipDto);
            await CreateAsync(createChampionshipDto, autoSave, cancellationToken: cancellationToken);
            var championshipDto = _mapper.Map<CreateChampionshipDto>(championship);
            return championshipDto;
        }

        public ValueTask<BaseResponse<ChampionDto>> UpdateAsync(CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
