using Application.Interfaces;
using Application.Interfaces.Persistence;
using AutoMapper;
using AutoMapper.Extensions.ExpressionMapping;
using Domain.Entities;
using Domain.Entities.Common;
using LanguageExt;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using Shared.Common.General;
using Shared.Exceptions;
using System.Linq.Expressions;

namespace Application.Implementations;

public abstract class BaseService<TEntity> : IBaseService<TEntity>
    where TEntity : BaseEntity<long>
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly ILogger<TEntity> _logger;


    public BaseService(IUnitOfWork unitOfWork, ILogger<TEntity> logger, IMapper mapper)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }


    #region Query

    public virtual async ValueTask<TDto?> GetByIdAsync<TDto>(long id, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default) where TDto : class
    {
        var entityResult = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id, null, cancellationToken);

        if (entityResult == null)
        {
            return null;
        }

        var entityDTO = customMapper != null ? customMapper(entityResult) : _mapper.Map<TDto>(entityResult);

        return entityDTO;
    }



    public virtual async ValueTask<TDto?> GetByIdAsync<TDto>(long id, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class
    {
        var includeEntityProperties = includeDTOProperties
       .Select(includeDtoProp => _mapper.MapExpression<Expression<Func<TEntity, object>>>(includeDtoProp))
       .ToArray();

        var entityResult = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id, includeEntityProperties, cancellationToken);

        if (entityResult == null)
        {
            return null;
        }

        var entityDTO = customMapper != null ? customMapper(entityResult) : _mapper.Map<TDto>(entityResult);

        return entityDTO;
    }


    public virtual async ValueTask<TDto?> FirstOrDefaultAsync<TDto>(Expression<Func<TDto, bool>> dtoPredicate, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class
    {

        var predicate = _mapper.MapExpression<Expression<Func<TEntity, bool>>>(dtoPredicate);
        var includeProperties = includeDTOProperties
          .Select(includeDtoProp => _mapper.MapExpression<Expression<Func<TEntity, object>>>(includeDtoProp))
          .ToArray();

        var entityResult = await _unitOfWork.Repository<TEntity>().FirstOrDefaultAsync(predicate, includeProperties, cancellationToken);

        if (entityResult is null)
            return null;

        return customMapper != null ? customMapper(entityResult) : _mapper.Map<TDto>(entityResult);


    }


    public virtual async ValueTask<IEnumerable<TDto>> GetAllAsync<TDto>(Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class
    {

        var includeProperties = includeDTOProperties
            .Select(includeDtoProp => _mapper.MapExpression<Expression<Func<TEntity, object>>>(includeDtoProp))
            .ToArray();

        var entityResult = await _unitOfWork.Repository<TEntity>().ListAsync(includeProperties, cancellationToken);

        if (entityResult is null)
            return null;

        return customMapper != null ? entityResult.Select(customMapper) : entityResult.Select(_mapper.Map<TDto>);
    }

    public virtual async ValueTask<PaginatedList<TDto>> GetPaginatedAsync<TDto>(PaginationSearchModel paginationParams, Func<TEntity, TDto>? customMapper = null, CancellationToken cancellationToken = default, params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class
    {
        // Map includeDTOProperties to the compatible entity properties
        var includeEntityProperties = includeDTOProperties
            .Select(includeDtoProp => _mapper.MapExpression<Expression<Func<TEntity, object>>>(includeDtoProp))
            .ToArray();

        // Retrieve paginated entities from the repository
        var entityResult = await _unitOfWork.Repository<TEntity>().PaginateAsync(paginationParams.PageIndex, paginationParams.PageSize, includeEntityProperties, cancellationToken);

        // Check if the entityResult is null
        if (entityResult == null)
        {
            return PaginatedList<TDto>.Create(Enumerable.Empty<TDto>(), 0, paginationParams.PageIndex, paginationParams.PageSize);
        }

        var mappedItems = customMapper != null
            ? entityResult.Items.Select(customMapper)
            : entityResult.Items.Select(entity => _mapper.Map<TDto>(entity));

        return PaginatedList<TDto>.Create(mappedItems, entityResult.TotalItems, paginationParams.PageIndex, paginationParams.PageSize);
    }


    public virtual async ValueTask<long> GetLongCountAsync(CancellationToken cancellationToken = default)
    {

        return await _unitOfWork.Repository<TEntity>().CountAsync(cancellationToken);
    }

    public async ValueTask<IEnumerable<TDto>> FindAsync<TDto>(
   Expression<Func<TDto, bool>> dtoPredicate,
   Func<TEntity, TDto>? customMapper = null,
   CancellationToken cancellationToken = default,
   params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class
    {
        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(dtoPredicate);
        var includeProperties = includeDTOProperties
          .Select(includeDtoProp => _mapper.MapExpression<Expression<Func<TEntity, object>>>(includeDtoProp))
          .ToArray();

        var entities = await _unitOfWork.Repository<TEntity>().ListAsync(predicate, includeProperties, cancellationToken);

        var dtos = customMapper != null
            ? entities.Select(customMapper)
            : _mapper.Map<IEnumerable<TDto>>(entities);

        return dtos;
    }


    public async ValueTask<PaginatedList<TDto>> GetPaginatedAsync<TDto>(
        Expression<Func<TDto, bool>> dtoPredicate,
        PaginationSearchModel paginationParams,
        Func<TEntity, TDto>? customMapper = null,
        CancellationToken cancellationToken = default,
        params Expression<Func<IQueryable<TDto>, IIncludableQueryable<TDto, object>>>[] includeDTOProperties) where TDto : class
    {
        // Map the DTO predicate to the entity predicate
        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(dtoPredicate);
        var includeProperties = includeDTOProperties
          .Select(includeDtoProp => _mapper.MapExpression<Expression<Func<TEntity, object>>>(includeDtoProp))
          .ToArray();
        // Get paginated entities
        var paginatedEntities = await _unitOfWork.Repository<TEntity>().PaginateAsync(
            predicate,
            paginationParams.PageIndex,
            paginationParams.PageSize,
            includeProperties,
            cancellationToken);

        var paginatedDtos = customMapper != null
            ? new PaginatedList<TDto>(paginatedEntities.Items.Select(customMapper), paginatedEntities.TotalItems, paginationParams.PageIndex, paginationParams.PageSize)
            : _mapper.Map<PaginatedList<TDto>>(paginatedEntities);

        return paginatedDtos;
    }


    #endregion


    #region Command

    public virtual ValueTask<Unit> CreateAsync<TDto>(TDto dto, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class
    {
        var mappedEntity = dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<TEntity>(dto);

        _unitOfWork.Repository<TEntity>().Create(mappedEntity);
        return new();
    }
    public virtual ValueTask<Unit> CreateRangeAsync<TDto>(IEnumerable<TDto> dtos, bool autoSave = true, Func<TDto, TEntity>? dtoToEntityMapper = null, CancellationToken cancellationToken = default) where TDto : class
    {

        var mappedEntites = dtos.Select(dto => dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<TEntity>(dto));

        _unitOfWork.Repository<TEntity>().CreateRange(mappedEntites);

        return new();
    }

    public virtual async ValueTask<Unit> UpdateAsync<TDto>(
      TDto dto,
      bool autoSave = true,
      Func<TDto, TEntity>? dtoToEntityMapper = null,
      Func<TEntity, TDto>? entityToDTOMapper = null,
      CancellationToken cancellationToken = default) where TDto : class
    {
        var entity = dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<TEntity>(dto);

        _unitOfWork.Repository<TEntity>().Update(entity);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }



    public virtual async ValueTask<Unit> UpdateRangeAsync<TDto>(
        IEnumerable<TDto> dtos,
        bool autoSave = true,
        Func<TDto, TEntity>? dtoToEntityMapper = null,
        CancellationToken cancellationToken = default) where TDto : class
    {
        var entitiesToUpdate = dtoToEntityMapper != null ? dtos.Select(dtoToEntityMapper) : dtos.Select(_mapper.Map<TEntity>);

        _unitOfWork.Repository<TEntity>().UpdateRange(entitiesToUpdate);
        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }


    #region Soft Delete

    public virtual async ValueTask<Unit> SoftDeleteAsync<TDto>(
        TDto dto,
        bool autoSave = true,
        Func<TDto, TEntity>? dtoToEntityMapper = null,
        Func<TEntity, TDto>? entityToDTOMapper = null,
        CancellationToken cancellationToken = default) where TDto : class
    {
        var entity = dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<TEntity>(dto);

        if (entity is SoftDeletableEntity<long> entityToDelete)
        {
            _unitOfWork.Repository<TEntity>().SoftDelete(entityToDelete);
        }


        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }

    public virtual async Task<Unit> SoftDeleteByIdAsync<TDto>(
        long id,
        bool autoSave = true,
        Func<TEntity, TDto>? entityToDTOMapper = null,
        CancellationToken cancellationToken = default) where TDto : class
    {
        // Retrieve the entity by ID
        var entity = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id);

        if (entity is null)
            throw new NotFoundException("Entity not found.");


        if (entity is SoftDeletableEntity<long> entityToDelete)
        {
            _unitOfWork.Repository<TEntity>().SoftDelete(entityToDelete);
        }
        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }

    public virtual async Task<Unit> SoftDeleteFirstAsync<TDto>(
        Expression<Func<TDto, bool>> dtoPredicate,
        bool autoSave = true,
        Func<TEntity, TDto>? entityToDTOMapper = null,
        CancellationToken cancellationToken = default) where TDto : class
    {


        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(dtoPredicate);

        var entity = await _unitOfWork.Repository<TEntity>().FirstOrDefaultAsync(predicate, null, cancellationToken);

        if (entity is null)
            throw new NotFoundException("Entity not found.");


        if (entity is SoftDeletableEntity<long> entityToDelete)
        {
            _unitOfWork.Repository<TEntity>().SoftDelete(entityToDelete);
        }

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new Unit();
    }

    public virtual async Task<Unit> SoftDeleteRangeAsync<TDto>(
       Expression<Func<TDto, bool>> dtoPredicate,
       bool autoSave = true,
       CancellationToken cancellationToken = default) where TDto : class
    {
        // Ensure TEntity is a SoftDeletableEntity
        if (!typeof(SoftDeletableEntity<long>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException("Cannot perform soft delete on this entity type.");
        }

        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(dtoPredicate);

        var entitiesToDelete = await _unitOfWork.Repository<TEntity>().ListAsync(predicate, null, cancellationToken);

        if (!entitiesToDelete.Any())
        {
            throw new NotFoundException("No entities found for deletion.");
        }

        var softDeletableEntities = entitiesToDelete.OfType<SoftDeletableEntity<long>>();

        _unitOfWork.Repository<TEntity>().SoftDeleteRange(softDeletableEntities);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }

    #endregion
    #region Hard Delete
    public virtual async ValueTask<Unit> HardDeleteAsync<TDto>(
    TDto dto,
    bool autoSave = true,
    Func<TDto, TEntity>? dtoToEntityMapper = null,
    Func<TEntity, TDto>? entityToDTOMapper = null,
    CancellationToken cancellationToken = default) where TDto : class
    {
        var entityToDelete = dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<TEntity>(dto);

        if (entityToDelete == null)
        {
            throw new NotFoundException("Entity not found for deletion.");
        }

        _unitOfWork.Repository<TEntity>().HardDelete(entityToDelete);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }

    public virtual async Task<Unit> HardDeleteByIdAsync<TDto>(
        long id,
        bool autoSave = true,
        Func<TEntity, TDto>? entityToDTOMapper = null,
        CancellationToken cancellationToken = default) where TDto : class
    {
        var entityToDelete = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id, null, cancellationToken);

        if (entityToDelete == null)
        {
            throw new NotFoundException("Entity not found for deletion.");
        }

        _unitOfWork.Repository<TEntity>().HardDelete(entityToDelete);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }

    public virtual async Task<Unit> HardDeleteFirstAsync<TDto>(
        Expression<Func<TDto, bool>> dtoPredicate,
        bool autoSave = true,
        Func<TEntity, TDto>? entityToDTOMapper = null,
        CancellationToken cancellationToken = default) where TDto : class
    {
        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(dtoPredicate);

        var entityToDelete = await _unitOfWork.Repository<TEntity>().FirstOrDefaultAsync(predicate, null, cancellationToken);

        if (entityToDelete == null)
        {
            throw new NotFoundException("Entity not found for deletion.");
        }

        _unitOfWork.Repository<TEntity>().HardDelete(entityToDelete);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }

    public virtual async Task<Unit> HardDeleteRangeAsync<TDto>(
        Expression<Func<TDto, bool>> dtoPredicate,
        bool autoSave = true,
        CancellationToken cancellationToken = default) where TDto : class
    {
        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(dtoPredicate);
        var entitiesToDelete = await _unitOfWork.Repository<TEntity>().ListAsync(predicate, null, cancellationToken);

        if (!entitiesToDelete.Any())
        {
            throw new NotFoundException("No entities found for deletion.");
        }

        _unitOfWork.Repository<TEntity>().HardDeleteRange(entitiesToDelete);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }

    #endregion

    #region Recover
    public virtual async ValueTask<TDto?> RecoverAsync<TDto>(
    TDto dto,
    bool autoSave = true,
    Func<TDto, TEntity>? dtoToEntityMapper = null,
    Func<TEntity, TDto>? entityToDTOMapper = null,
    CancellationToken cancellationToken = default) where TDto : class
    {
        var entityToRecover = dtoToEntityMapper != null ? dtoToEntityMapper(dto) : _mapper.Map<TEntity>(dto);

        if (entityToRecover is not SoftDeletableEntity<long> softDeletableEntity)
        {
            throw new InvalidOperationException("Entity does not support recovery.");
        }

        if (entityToRecover is null)
        {
            throw new NotFoundException("Entity not found for recovery.");
        }

        _unitOfWork.Repository<TEntity>().Recover(softDeletableEntity);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Return the mapped DTO
        return entityToDTOMapper != null ? entityToDTOMapper(entityToRecover) : _mapper.Map<TDto>(entityToRecover);
    }

    public virtual async Task<TDto?> RecoverByIdAsync<TDto>(long id, bool autoSave = true, Func<TEntity, TDto>? entityToDTOMapper = null, CancellationToken cancellationToken = default) where TDto : class
    {
        var entityToRecover = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id, null, cancellationToken);

        if (entityToRecover is not SoftDeletableEntity<long> softDeletableEntity)
        {
            throw new InvalidOperationException("Entity does not support recovery.");
        }

        if (entityToRecover == null)
        {
            throw new NotFoundException("Entity not found for recovery.");
        }

        _unitOfWork.Repository<TEntity>().Recover(softDeletableEntity);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Return the mapped DTO
        return entityToDTOMapper != null ? entityToDTOMapper(entityToRecover) : _mapper.Map<TDto>(entityToRecover);
    }

    public virtual async Task<TDto?> RecoverFirstAsync<TDto>(
        Expression<Func<TDto, bool>> dtoPredicate,
        bool autoSave = true,
        Func<TEntity, TDto>? entityToDTOMapper = null,
        CancellationToken cancellationToken = default) where TDto : class
    {
        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(dtoPredicate);

        var entityToRecover = await _unitOfWork.Repository<TEntity>().FirstOrDefaultAsync(predicate, null, cancellationToken);

        if (entityToRecover is not SoftDeletableEntity<long> softDeletableEntity)
        {
            throw new InvalidOperationException("Entity does not support recovery.");
        }

        if (entityToRecover == null)
        {
            throw new NotFoundException("Entity not found for recovery.");
        }

        _unitOfWork.Repository<TEntity>().Recover(softDeletableEntity);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return entityToDTOMapper != null ? entityToDTOMapper(entityToRecover) : _mapper.Map<TDto>(entityToRecover);
    }

    public virtual async Task<Unit> RecoverRangeAsync<TDto>(
        Expression<Func<TDto, bool>> dtoPredicate,
        bool autoSave = true,
        CancellationToken cancellationToken = default) where TDto : class
    {
        if (!typeof(SoftDeletableEntity<long>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException("Cannot perform Recover  on this entity type.");
        }

        var predicate = _mapper.Map<Expression<Func<TEntity, bool>>>(dtoPredicate);

        var entitiesToRecover = await _unitOfWork.Repository<TEntity>().ListAsync(predicate, null, cancellationToken);

        if (!entitiesToRecover.Any())
        {
            throw new NotFoundException("No entities found for recovery.");
        }

        _unitOfWork.Repository<TEntity>().RecoverRange(entitiesToRecover.Cast<SoftDeletableEntity<long>>());

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }



    #endregion

    #endregion
}
