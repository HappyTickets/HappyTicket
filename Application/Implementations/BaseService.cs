using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Domain.Entities;
using FluentValidation;
using Humanizer;
using LanguageExt;
using LanguageExt.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Shared.Common.General;
using Shared.Exceptions;
using Shared.Extensions;
using Shared.ResourceFiles;
using System.Linq.Expressions;

namespace Application.Implementations;

public class BaseService<Tentity, Tdto> : IBaseService<Tentity, Tdto>
    where Tentity : BaseEntity
    where Tdto : class
{
    protected readonly IMapper _mapper;
    protected readonly IMemoryCache _cache;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly ILogger<Tentity> _logger;
    protected readonly IValidator<Tdto> _validator;
    protected readonly IStringLocalizer<Resource> _localizer;

    public BaseService(IUnitOfWork unitOfWork, ILogger<Tentity> logger, IMemoryCache cache, IMapper mapper, IValidator<Tdto> validator, IStringLocalizer<Resource> localizer)
    {
        _mapper = mapper;
        _cache = cache;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _validator = validator;
        _localizer = localizer;
    }


    #region Query

    public virtual async ValueTask<Result<Tdto>> GetByIdAsync(Guid id, bool useCache = true, Func<Tentity, Tdto>? customMapper = null, CancellationToken cancellationToken = default)
    {
        try
        {
            var cacheKey = $"{typeof(Tentity).Name}_{id}";
            if (useCache && _cache.TryGetValue(cacheKey, out Tdto? dto) && dto != null)
            {
                return dto;
            }

            var entityResult = await _unitOfWork.Repository<Tentity>().GetByIdAsync(id, cancellationToken);
            return entityResult.Match(
                succ =>
                {
                    var entityDTO = customMapper != null ? customMapper(succ) : _mapper.Map<Tdto>(succ);
                    _cache.Set(cacheKey, entityDTO);
                    return entityDTO;
                },
                fail =>
                {
                    return (fail is NotFoundException) ?
                        new Result<Tdto>(new NotFoundException([new() { Title = Resource.NotFound, Message = Resource.NotFound_Message.ToString().Replace("{type}", _localizer[typeof(Tentity).Name]) }])) :
                            new(fail);
                }
            );
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }


    public virtual async ValueTask<Result<Tdto>> GetByIdAsync(Guid id, bool useCache = true,
                                                              Func<Tentity, Tdto>? customMapper = null,
                                                              CancellationToken cancellationToken = default,
                                                              params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties)
    {
        try
        {
            var cacheKey = $"{typeof(Tentity).Name}_{id}";
            if (useCache && _cache.TryGetValue(cacheKey, out Tdto? dto) && dto != null)
            {
                return dto;
            }

            var includeProperties = includeDTOProperties.Select(_mapper.MapExpression<Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>>).ToArray();

            var entityResult = await _unitOfWork.Repository<Tentity>().GetByIdAsync(id, cancellationToken, includeProperties);
            return entityResult.Match(
                succ =>
                {
                    var entityDTO = customMapper != null ? customMapper(succ) : _mapper.Map<Tdto>(succ);
                    _cache.Set(cacheKey, entityDTO);
                    return entityDTO;
                },
                fail =>
                {
                    return (fail is NotFoundException) ?
                        new Result<Tdto>(new NotFoundException([new() { Title = Resource.NotFound, Message = Resource.NotFound_Message.ToString().Replace("{type}", _localizer[typeof(Tentity).Name]) }])) :
                            new(fail);
                }
            );
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public virtual async ValueTask<Result<Tdto>> FirstOrDefaultAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool useCache = true,
                                                                     Func<Tentity, Tdto>? customMapper = null,
                                                                     CancellationToken cancellationToken = default,
                                                                     params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties)
    {
        try
        {
            var cacheKey = $"All_{typeof(Tentity).Name.Pluralize()}";
            if (useCache && _cache.TryGetValue(cacheKey, out IEnumerable<Tdto>? cachedDTOs) && cachedDTOs != null)
            {
                var dto = cachedDTOs.FirstOrDefault(dtoPredicate.Compile());
                if (dto != null) return new(dto);
            }

            var predicate = _mapper.MapExpression<Expression<Func<Tentity, bool>>>(dtoPredicate);
            var includeProperties = includeDTOProperties.Select(_mapper.MapExpression<Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>>).ToArray();

            var entityResult = await _unitOfWork.Repository<Tentity>().FirstOrDefaultAsync(predicate, cancellationToken, includeProperties);
            return entityResult.Match(
                succ =>
                {
                    var dto = customMapper != null ? customMapper(succ) : _mapper.Map<Tdto>(succ);
                    _cache.Set(cacheKey, dto);
                    return dto;
                },
                fail =>
                {
                    return (fail is NotFoundException) ?
                        new Result<Tdto>(new NotFoundException([new() { Title = Resource.NotFound, Message = Resource.NotFound_Message.ToString().Replace("{type}", _localizer[typeof(Tentity).Name]) }])) :
                            new(fail);
                }
            );
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public virtual async ValueTask<Result<IEnumerable<Tdto>>> FindAsync(Expression<Func<Tdto, bool>> dtoPredicate, bool useCache = true,
                                                                        Func<Tentity, Tdto>? customMapper = null,
                                                                        CancellationToken cancellationToken = default,
                                                                        params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties)
    {
        try
        {
            var cacheKey = $"All_{typeof(Tentity).Name.Pluralize()}";
            if (useCache && _cache.TryGetValue(cacheKey, out IEnumerable<Tdto>? cachedDTOs) && cachedDTOs != null)
            {
                var dto = cachedDTOs.Where(dtoPredicate.Compile());
                if (dto != null) return new(dto);
            }

            var predicate = _mapper.MapExpression<Expression<Func<Tentity, bool>>>(dtoPredicate);
            var includeProperties = includeDTOProperties.Select(_mapper.MapExpression<Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>>).ToArray();

            var entityResult = await _unitOfWork.Repository<Tentity>().FindAsync(predicate, cancellationToken, includeProperties);
            return entityResult.Match(
                succ =>
                {
                    var dtos = customMapper != null ? succ.Select(customMapper) : succ.Select(_mapper.Map<Tdto>);
                    _cache.Set(cacheKey, dtos);
                    return new(dtos);
                },
                fail =>
                {
                    return (fail is NotFoundException) ?
                        new Result<IEnumerable<Tdto>>(new NotFoundException([new() { Title = Resource.NotFound, Message = Resource.NotFound_Message.ToString().Replace("{type}", _localizer[typeof(Tentity).Name.Pluralize()]) }])) :
                            new(fail);
                }
            );
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public virtual async ValueTask<Result<IEnumerable<Tdto>>> GetAllAsync(bool useCache = true, Func<Tentity, Tdto>? customMapper = null,
                                                                          CancellationToken cancellationToken = default,
                                                                          params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties)
    {
        try
        {
            var cacheKey = $"All_{typeof(Tentity).Name.Pluralize()}";
            if (useCache && _cache.TryGetValue(cacheKey, out IEnumerable<Tdto>? cachedDTOs) && cachedDTOs != null)
            {
                return new(cachedDTOs);
            }

            var entityResult = await _unitOfWork.Repository<Tentity>().GetAllAsync(cancellationToken, includeDTOProperties.Select(_mapper.MapExpression<Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>>).ToArray());
            return entityResult.Match(
                succ =>
                {
                    var dtos = customMapper != null ? succ.Select(customMapper) : succ.Select(_mapper.Map<Tdto>);
                    _cache.Set(cacheKey, dtos);
                    return new(dtos);
                },
                fail =>
                {
                    return (fail is NotFoundException) ?
                        new Result<IEnumerable<Tdto>>(new NotFoundException([new() { Title = Resource.NotFound, Message = Resource.NotFoundInDB_Message.ToString().Replace("{type}", _localizer[typeof(Tentity).Name.Pluralize()]) }])) :
                            new(fail);
                }
            );
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public virtual async ValueTask<Result<IEnumerable<Tdto>>> GetPaginatedAsync(PaginationParams paginationParams,
                                                                                bool useCache = true,
                                                                                Func<Tentity, Tdto>? customMapper = null,
                                                                                CancellationToken cancellationToken = default,
                                                                                params Expression<Func<IQueryable<Tdto>, IIncludableQueryable<Tdto, object>>>[] includeDTOProperties)
    {
        try
        {
            var cacheKey = $"All_{typeof(Tentity).Name.Pluralize()}";
            if (useCache && _cache.TryGetValue(cacheKey, out IEnumerable<Tdto>? cachedDTOs) && cachedDTOs != null)
            {
                return new(cachedDTOs);
            }

            var entityResult = await _unitOfWork.Repository<Tentity>().GetPaginatedAsync(paginationParams, cancellationToken, includeDTOProperties.Select(_mapper.MapExpression<Expression<Func<IQueryable<Tentity>, IIncludableQueryable<Tentity, object>>>>).ToArray());
            return entityResult.Match(
                succ =>
                {
                    var dtos = customMapper != null ? succ.Select(customMapper) : succ.Select(_mapper.Map<Tdto>);
                    _cache.Set(cacheKey, dtos);
                    return new(dtos);
                },
                fail =>
                {
                    return (fail is NotFoundException) ?
                        new Result<IEnumerable<Tdto>>(new NotFoundException([new() { Title = Resource.NotFound, Message = Resource.NotFoundInDB_Message.ToString().Replace("{type}", _localizer[typeof(Tentity).Name.Pluralize()]) }])) :
                            new(fail);
                }
            );
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public virtual async ValueTask<Result<long>> GetLongCountAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            var count = await _unitOfWork.Repository<Tentity>().GetLongCountAsync(cancellationToken);

            return count.Match(
                succ =>
                {
                    return count;
                },
                fail =>
                {
                    return (fail is NotFoundException) ? new Result<long>(0) : new(fail);
                }
            );
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    #endregion


    #region Command

    public virtual async ValueTask<Result<Tdto>> CreateAsync(Tdto dto, bool autoSave = true,
                                                             Func<Tdto, Tentity>? dtoToEntityMapper = null,
                                                             Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                             CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return new Result<Tdto>(new EntityValidationException(validationResult.Errors));
            }

            var createdEntityResult = await _unitOfWork.Repository<Tentity>().CreateAsync(dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<Tentity>(dto), cancellationToken);

            return await createdEntityResult.Match(
                async createdEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken))
                                                            .Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(createdEntitySucc) : _mapper.Map<Tdto>(createdEntitySucc))
                                                                : (entityToDTOMapper != null ? entityToDTOMapper(createdEntitySucc) : _mapper.Map<Tdto>(createdEntitySucc)),
                async createdEntityFail => await createdEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async ValueTask<Result<Unit>> CreateRangeAsync(IEnumerable<Tdto> dtos, bool autoSave = true,
                                                                  Func<Tdto, Tentity>? dtoToEntityMapper = null,
                                                                  CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResults = dtos.Select(dto => _validator.Validate(dto));
            if (validationResults.Any(validationResult => !validationResult.IsValid))
            {
                return new Result<Unit>(new EntityValidationException(validationResults.SelectMany(validationResult => validationResult.Errors)));
            }

            var createdEntitiesResult = await _unitOfWork.Repository<Tentity>().CreateRangeAsync(dtos.Select(dto => dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<Tentity>(dto)), cancellationToken);

                var result = await createdEntitiesResult.Match(
                async createdEntitiesSucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => createdEntitiesSucc) : createdEntitiesSucc,
                async createdEntitiesFail => await createdEntitiesFail.ToResultAsync<Unit>(cancellationToken));
            return result;
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public virtual async ValueTask<Result<Tdto>> UpdateAsync(Tdto dto, bool autoSave = true,
                                                             Func<Tdto, Tentity>? dtoToEntityMapper = null,
                                                             Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                             CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
                return new Result<Tdto>(new EntityValidationException(validationResult.Errors));

            Tentity entity = dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<Tentity>(dto);

            if (entity.Id == Guid.Empty) return new Result<Tdto>(new EntityValidationException(validationResult.Errors));

            var updatedEntityResult = _unitOfWork.Repository<Tentity>().Update(dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<Tentity>(dto));

            return await updatedEntityResult.Match(
                async updatedEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(updatedEntitySucc) : _mapper.Map<Tdto>(updatedEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(updatedEntitySucc) : _mapper.Map<Tdto>(updatedEntitySucc),
                async updatedEntityFail => await updatedEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async ValueTask<Result<Unit>> UpdateRangeAsync(IEnumerable<Tdto> dtos, bool autoSave = true,
                                                                  Func<Tdto, Tentity>? dtoToEntityMapper = null,
                                                                  CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResults = dtos.Select(dto => _validator.Validate(dto));
            if (validationResults.Any(validationResult => !validationResult.IsValid))
            {
                return new Result<Unit>(new EntityValidationException(validationResults.SelectMany(validationResult => validationResult.Errors)));
            }

            var updatedEntitiesResult = _unitOfWork.Repository<Tentity>().UpdateRange(dtos.Select(dto => dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<Tentity>(dto)));

            return await updatedEntitiesResult.Match(
                async updatedEntitiesSucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => updatedEntitiesSucc) : updatedEntitiesSucc,
                async updatedEntitiesFail => await updatedEntitiesFail.ToResultAsync<Unit>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Unit>> UpdateRangeAsync(Expression<Func<Tdto, bool>> dtoPredicate,
                                                             Expression<Func<Tdto, Tdto>> updateDTOFactory,
                                                             bool autoSave = true,
                                                             CancellationToken cancellationToken = default)
    {
        try
        {
            var predicate = _mapper.Map<Expression<Func<Tentity, bool>>>(dtoPredicate);
            var updateFactory = _mapper.Map<Expression<Func<Tentity, Tentity>>>(updateDTOFactory);

            var updatedEntitiesResult = await _unitOfWork.Repository<Tentity>().UpdateRangeFactoryAsync(predicate, updateFactory, cancellationToken);

            return await updatedEntitiesResult.Match(
                async updatedEntitiesSucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => updatedEntitiesSucc) : updatedEntitiesSucc,
                async updatedEntitiesFail => await updatedEntitiesFail.ToResultAsync<Unit>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new Result<Unit>(ex);
        }
    }

    public virtual async ValueTask<Result<Tdto>> RecoverAsync(Tdto dto, bool autoSave = true,
                                                              Func<Tdto, Tentity>? dtoToEntityMapper = null,
                                                              Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                              CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return new Result<Tdto>(new EntityValidationException(validationResult.Errors));
            }

            var recoveredEntityResult = _unitOfWork.Repository<Tentity>().Recover(dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<Tentity>(dto));

            return await recoveredEntityResult.Match(
                async recoveredEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(recoveredEntitySucc) : _mapper.Map<Tdto>(recoveredEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(recoveredEntitySucc) : _mapper.Map<Tdto>(recoveredEntitySucc),
                async recoveredEntityFail => await recoveredEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Tdto>> RecoverByIdAsync(Guid id, bool autoSave = true,
                                                             Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                             CancellationToken cancellationToken = default)
    {
        try
        {
            var recoveredEntityResult = await _unitOfWork.Repository<Tentity>().RecoverByIdAsync(id, cancellationToken);

            return await recoveredEntityResult.Match(
                async recoveredEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(recoveredEntitySucc) : _mapper.Map<Tdto>(recoveredEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(recoveredEntitySucc) : _mapper.Map<Tdto>(recoveredEntitySucc),
                async recoveredEntityFail => await recoveredEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Tdto>> RecoverFirstAsync(Expression<Func<Tdto, bool>> dtoPredicate,
                                                              bool autoSave = true,
                                                              Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                              CancellationToken cancellationToken = default)
    {
        try
        {
            var predicate = _mapper.Map<Expression<Func<Tentity, bool>>>(dtoPredicate);

            var recoveredEntityResult = await _unitOfWork.Repository<Tentity>().RecoverFirstAsync(predicate, cancellationToken);

            return await recoveredEntityResult.Match(
                async recoveredEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(recoveredEntitySucc) : _mapper.Map<Tdto>(recoveredEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(recoveredEntitySucc) : _mapper.Map<Tdto>(recoveredEntitySucc),
                async recoveredEntityFail => await recoveredEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Unit>> RecoverRangeAsync(Expression<Func<Tdto, bool>> dtoPredicate,
                                                              bool autoSave = true,
                                                              CancellationToken cancellationToken = default)
    {
        try
        {
            var predicate = _mapper.Map<Expression<Func<Tentity, bool>>>(dtoPredicate);

            var recoveredEntitiesResult = await _unitOfWork.Repository<Tentity>().RecoverRangeAsync(predicate, cancellationToken);

            return await recoveredEntitiesResult.Match(
                async recoveredEntitiesSucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => recoveredEntitiesSucc) : recoveredEntitiesSucc,
                async recoveredEntitiesFail => await recoveredEntitiesFail.ToResultAsync<Unit>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public virtual async ValueTask<Result<Tdto>> SoftDeleteAsync(Tdto dto, bool autoSave = true,
                                                                 Func<Tdto, Tentity>? dtoToEntityMapper = null,
                                                                 Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                                 CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return new Result<Tdto>(new EntityValidationException(validationResult.Errors));
            }

            var softDeletedEntityResult = _unitOfWork.Repository<Tentity>().SoftDelete(dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<Tentity>(dto));

            return await softDeletedEntityResult.Match(
                async softDeletedEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(softDeletedEntitySucc) : _mapper.Map<Tdto>(softDeletedEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(softDeletedEntitySucc) : _mapper.Map<Tdto>(softDeletedEntitySucc),
                async softDeletedEntityFail => await softDeletedEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Tdto>> SoftDeleteByIdAsync(Guid id, bool autoSave = true,
                                                                Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                                CancellationToken cancellationToken = default)
    {
        try
        {
            var softDeletedEntityResult = await _unitOfWork.Repository<Tentity>().SoftDeleteByIdAsync(id, cancellationToken);

            return await softDeletedEntityResult.Match(
                async softDeletedEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(softDeletedEntitySucc) : _mapper.Map<Tdto>(softDeletedEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(softDeletedEntitySucc) : _mapper.Map<Tdto>(softDeletedEntitySucc),
                async softDeletedEntityFail => await softDeletedEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Tdto>> SoftDeleteFirstAsync(Expression<Func<Tdto, bool>> dtoPredicate,
                                                                 bool autoSave = true,
                                                                 Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                                 CancellationToken cancellationToken = default)
    {
        try
        {
            var predicate = _mapper.Map<Expression<Func<Tentity, bool>>>(dtoPredicate);

            var softDeletedEntityResult = await _unitOfWork.Repository<Tentity>().SoftDeleteFirstAsync(predicate, cancellationToken);

            return await softDeletedEntityResult.Match(
                async softDeletedEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(softDeletedEntitySucc) : _mapper.Map<Tdto>(softDeletedEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(softDeletedEntitySucc) : _mapper.Map<Tdto>(softDeletedEntitySucc),
                async softDeletedEntityFail => await softDeletedEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Unit>> SoftDeleteRangeAsync(Expression<Func<Tdto, bool>> dtoPredicate,
                                                                 bool autoSave = true,
                                                                 CancellationToken cancellationToken = default)
    {
        try
        {
            var predicate = _mapper.Map<Expression<Func<Tentity, bool>>>(dtoPredicate);

            var softDeletedEntitiesResult = await _unitOfWork.Repository<Tentity>().SoftDeleteRangeAsync(predicate, cancellationToken);

            return await softDeletedEntitiesResult.Match(
                async softDeletedEntitiesSucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => softDeletedEntitiesSucc) : softDeletedEntitiesSucc,
                async softDeletedEntitiesFail => await softDeletedEntitiesFail.ToResultAsync<Unit>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    public virtual async ValueTask<Result<Tdto>> HardDeleteAsync(Tdto dto, bool autoSave = true,
                                                                 Func<Tdto, Tentity>? dtoToEntityMapper = null,
                                                                 Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                                 CancellationToken cancellationToken = default)
    {
        try
        {
            var validationResult = _validator.Validate(dto);
            if (!validationResult.IsValid)
            {
                return new Result<Tdto>(new EntityValidationException(validationResult.Errors));
            }

            var hardDeletedEntityResult = _unitOfWork.Repository<Tentity>().HardDelete(dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<Tentity>(dto));

            return await hardDeletedEntityResult.Match(
                async hardDeletedEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(hardDeletedEntitySucc) : _mapper.Map<Tdto>(hardDeletedEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(hardDeletedEntitySucc) : _mapper.Map<Tdto>(hardDeletedEntitySucc),
                async hardDeletedEntityFail => await hardDeletedEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Tdto>> HardDeleteByIdAsync(Guid id, bool autoSave = true,
                                                                Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                                CancellationToken cancellationToken = default)
    {
        try
        {
            var hardDeletedEntityResult = await _unitOfWork.Repository<Tentity>().HardDeleteByIdAsync(id, cancellationToken);

            return await hardDeletedEntityResult.Match(
                async hardDeletedEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(hardDeletedEntitySucc) : _mapper.Map<Tdto>(hardDeletedEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(hardDeletedEntitySucc) : _mapper.Map<Tdto>(hardDeletedEntitySucc),
                async hardDeletedEntityFail => await hardDeletedEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Tdto>> HardDeleteFirstAsync(Expression<Func<Tdto, bool>> dtoPredicate,
                                                                 bool autoSave = true,
                                                                 Func<Tentity, Tdto>? entityToDTOMapper = null,
                                                                 CancellationToken cancellationToken = default)
    {
        try
        {
            var predicate = _mapper.Map<Expression<Func<Tentity, bool>>>(dtoPredicate);

            var hardDeletedEntityResult = await _unitOfWork.Repository<Tentity>().HardDeleteFirstAsync(predicate, cancellationToken);

            return await hardDeletedEntityResult.Match(
                async hardDeletedEntitySucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => entityToDTOMapper != null ? entityToDTOMapper(hardDeletedEntitySucc) : _mapper.Map<Tdto>(hardDeletedEntitySucc))
                    : entityToDTOMapper != null ? entityToDTOMapper(hardDeletedEntitySucc) : _mapper.Map<Tdto>(hardDeletedEntitySucc),
                async hardDeletedEntityFail => await hardDeletedEntityFail.ToResultAsync<Tdto>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }
    public virtual async Task<Result<Unit>> HardDeleteRangeAsync(Expression<Func<Tdto, bool>> dtoPredicate,
                                                                 bool autoSave = true,
                                                                 CancellationToken cancellationToken = default)
    {
        try
        {
            var predicate = _mapper.Map<Expression<Func<Tentity, bool>>>(dtoPredicate);

            var hardDeletedEntitiesResult = _unitOfWork.Repository<Tentity>().HardDeleteRange(predicate);

            return await hardDeletedEntitiesResult.Match(
                async hardDeletedEntitiesSucc => autoSave ? (await _unitOfWork.SaveChangesAsync(cancellationToken)).Map(saveSucc => hardDeletedEntitiesSucc) : hardDeletedEntitiesSucc,
                async hardDeletedEntitiesFail => await hardDeletedEntitiesFail.ToResultAsync<Unit>(cancellationToken));
        }
        catch (Exception ex)
        {
            return new(ex);
        }
    }

    #endregion
}
