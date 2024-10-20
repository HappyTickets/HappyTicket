using Application.Common.Implementations;
using Application.Common.Interfaces.Persistence;
using Application.Interfaces;
using AutoMapper;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Shared.Common;
using Shared.DTOs.Champion;
using Shared.DTOs.ChampionDtos;
using Shared.ResourceFiles;
using System.Net;

namespace Application.Implementations
{
    public class ChampionService : BaseService<Championship>, IChampionService
    {
        public ChampionService(IUnitOfWork unitOfWork, ILogger<Championship> logger, IMapper mapper) : base(unitOfWork, logger, mapper) { }
        public async ValueTask<BaseResponse<CreateChampionshipDto>> CreateChampionAsync(CreateChampionshipDto createChampionshipDto, bool autoSave = true, CancellationToken cancellationToken = default)
        {
            var championship = _mapper.Map<Championship>(createChampionshipDto);
            await CreateAsync(createChampionshipDto, autoSave, cancellationToken: cancellationToken);
            var championshipDto = _mapper.Map<CreateChampionshipDto>(championship);
            return championshipDto;
        }
        public async ValueTask<BaseResponse<UpdateChampionshipDto>> UpdateAsync(UpdateChampionshipDto updateChampionshipDto, CancellationToken cancellationToken = default)
        {
            var championship = await GetByIdAsync<Championship>(updateChampionshipDto.Id);
            if (championship == null)
            {
                return new BaseResponse<UpdateChampionshipDto>
                {
                    Status = HttpStatusCode.BadRequest,
                    Title = Resource.ChampionshipNotFound
                };
            }
            _mapper.Map(updateChampionshipDto, championship);
            await UpdateAsync(championship);
            return updateChampionshipDto;
        }
        public async ValueTask<BaseResponse<IEnumerable<GetChampionshipDto>>> GetAllChampionshipsAsync(CancellationToken cancellationToken = default)
        {
            var championships = await GetAllAsync<GetChampionshipDto>();
            if (championships != null && championships.Any())
                return new BaseResponse<IEnumerable<GetChampionshipDto>>(championships.ToList());
            return new BaseResponse<IEnumerable<GetChampionshipDto>>
            {
                Status = HttpStatusCode.NotFound,
                Title = Resource.NoChampionshipFound
            };
        }
        public async ValueTask<BaseResponse<GetChampionshipDto>> GetChampionshipByIdAsync(long championshipId)
        {
            var championship = await GetByIdAsync<GetChampionshipDto>(championshipId);
            if (championship == null)
            {
                return new BaseResponse<GetChampionshipDto>
                {
                    Status = HttpStatusCode.NotFound,
                    Title = Resource.NoChampionshipFound
                };
            }
            return new BaseResponse<GetChampionshipDto>(championship);
        }
        public async ValueTask<BaseResponse<object?>> DeleteChampionAsync(long championshipId, CancellationToken cancellationToken = default)
        {
            var championship = await GetByIdAsync<Championship>(championshipId, cancellationToken);
            if (championship == null)
            {
                return new BaseResponse<object?>
                {
                    Status = HttpStatusCode.NotFound,
                    Title = Resource.ChampionshipNotFound
                };
            }
            await HardDeleteAsync(championship);
            return new BaseResponse<object?>
            {
                Status = HttpStatusCode.OK,
                Title = Resource.ChampionshipDeleted
            };
        }
    }
}
