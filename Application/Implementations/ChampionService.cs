using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using Domain.Entities;
using FluentValidation;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shared.DTOs.Champion;
using Shared.Extensions;
using Shared.ResourceFiles;

namespace Application.Implementations
{
    public class ChampionService : BaseService<Champion, ChampionDto>, IChampionService
    {
        private readonly IValidator<CreateOrUpdateChampionDto> _createOrUpdateValidator;

        public ChampionService(IUnitOfWork unitOfWork, ILogger<Champion> logger, IMemoryCache cache, IMapper mapper, IValidator<ChampionDto> validator, IStringLocalizer<Resource> localizer, IValidator<CreateOrUpdateChampionDto> createOrUpdateValidator) : base(unitOfWork, logger, cache, mapper, validator, localizer)
        {
            _createOrUpdateValidator = createOrUpdateValidator;
        }

        public async Task<Result<ChampionDto>> CreateAsync(CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = _createOrUpdateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return new(new ValidationException(validationResult.Errors));

                var championRepo = _unitOfWork.Repository<Champion>();
                
                var champion = _mapper.Map<Champion>(dto);
                if(dto.SponsorsIds != null) 
                {
                    champion.ChampionSponsors = dto.SponsorsIds.Select(id => new ChampionSponsor
                    {
                        SponsorId = id
                    }).ToArray();
                }

                var result = await championRepo.CreateAsync(champion);
                return await result.Match(
                    async champion =>
                    {
                        await _unitOfWork.SaveChangesAsync();
                        return _mapper.Map<ChampionDto>(champion).ToResult();
                    },
                    async ex => await ex.ToResultAsync<ChampionDto>());
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<ChampionDto>> UpdateAsync(Guid id, CreateOrUpdateChampionDto dto, CancellationToken cancellationToken = default)
        {
            try
            {
                var validationResult = _createOrUpdateValidator.Validate(dto);
                if (!validationResult.IsValid)
                    return new(new ValidationException(validationResult.Errors));

                var championRepo = _unitOfWork.Repository<Champion>();
                var championSponsorRepo = _unitOfWork.Repository <ChampionSponsor>();
                var championResult = await championRepo.GetByIdAsync(id, cancellationToken);
                return await championResult.Match(
                    async champion =>
                    {
                        _mapper.Map(dto, champion);
                        var result = championRepo.Update(champion);
                        return await result.Match(
                            async champion =>
                            {
                                championSponsorRepo.HardDeleteRange(cs => cs.ChampionId == id);
                                if (dto.SponsorsIds != null)
                                {
                                    var championSponsors = dto.SponsorsIds.Select(i => new ChampionSponsor
                                    {
                                        ChampionId = id,
                                        SponsorId = i
                                    });
                                    await championSponsorRepo.CreateRangeAsync(championSponsors);
                                }
                                await _unitOfWork.SaveChangesAsync();
                                return _mapper.Map<ChampionDto>(champion).ToResult();
                            },
                            async ex => await ex.ToResultAsync<ChampionDto>());
                    },
                    async ex => await ex.ToResultAsync<ChampionDto>()
                    );
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<ChampionDto>> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            try
            {
                var matchRepo = _unitOfWork.Repository<Match>();
                var championRepo = _unitOfWork.Repository<Champion>();

                if (matchRepo.Query().Any(m => m.ChampionId == id))
                    return new(new Exception(Resource.Champion_With_Match_Deletion_Failure));

                var result = await championRepo.HardDeleteByIdAsync(id, cancellationToken);
                return await result.Match(
                    async champion =>
                    {
                        await _unitOfWork.SaveChangesAsync();
                        return _mapper.Map<ChampionDto>(champion).ToResult();
                    },
                    async ex => await ex.ToResultAsync<ChampionDto>());
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async  Task<Result<ChampionDto>> GetByIdAsync(Guid id, bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.GetByIdAsync(id: id, useCache: useCache, cancellationToken: cancellationToken, includeDTOProperties: q => q.Include(c => c.ChampionSponsors).ThenInclude(cs => cs.Sponsor));
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }

        public async Task<Result<IEnumerable<ChampionDto>>> GetAllAsync(bool useCache = true, CancellationToken cancellationToken = default)
        {
            try
            {
                return await base.GetAllAsync(useCache: useCache, cancellationToken: cancellationToken, includeDTOProperties: q => q.Include(c => c.ChampionSponsors).ThenInclude(cs => cs.Sponsor));
            }
            catch (Exception ex)
            {
                return new(ex);
            }
        }
    }
}
