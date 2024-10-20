using Application.Common.Interfaces.Persistence;
using Application.Common.Interfaces.Services;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Common;
using LanguageExt;
using Microsoft.Extensions.Logging;
using Shared.Common.General;
using Shared.Exceptions;
using Shared.ResourceFiles;
using System.Linq.Expressions;

namespace Application.Common.Implementations;

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


    public virtual async ValueTask<TDto?> GetByIdAsync<TDto>(long id, CancellationToken cancellationToken = default, IEnumerable<string>? includes = null) where TDto : class
    {

        var entityResult = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id, includes, cancellationToken);

        if (entityResult == null)
        {
            return null;
        }

        var entityDTO = _mapper.Map<TDto>(entityResult);

        return entityDTO;
    }


    public virtual async ValueTask<TDto?> FirstOrDefaultAsync<TDto>(Expression<Func<TEntity, bool>> predicate, CancellationToken cancellationToken = default, IEnumerable<string>? includes = null) where TDto : class
    {
        var entityResult = await _unitOfWork.Repository<TEntity>().FirstOrDefaultAsync(predicate, includes, cancellationToken);

        if (entityResult is null)
            return null;

        return _mapper.Map<TDto>(entityResult);


    }


    public virtual async ValueTask<IEnumerable<TDto>> GetAllAsync<TDto>(CancellationToken cancellationToken = default, IEnumerable<string>? includes = null) where TDto : class
    {

        var entityResult = await _unitOfWork.Repository<TEntity>().ListAsync(includes, cancellationToken);

        if (entityResult is null)
            return null;

        return entityResult.Select(_mapper.Map<TDto>);
    }

    public virtual async ValueTask<PaginatedList<TDto>> GetPaginatedAsync<TDto>(PaginationParams paginationParams, CancellationToken cancellationToken = default, IEnumerable<string>? includes = null) where TDto : class
    {
        // Retrieve paginated entities from the repository
        var entityResult = await _unitOfWork.Repository<TEntity>().PaginateAsync(paginationParams.PageIndex, paginationParams.PageSize, includes, cancellationToken);

        // Check if the entityResult is null
        if (entityResult == null)
        {
            return PaginatedList<TDto>.Create(Enumerable.Empty<TDto>(), 0, paginationParams.PageIndex, paginationParams.PageSize);
        }

        var mappedItems = entityResult.Items.Select(entity => _mapper.Map<TDto>(entity));

        return PaginatedList<TDto>.Create(mappedItems, entityResult.TotalItems, paginationParams.PageIndex, paginationParams.PageSize);
    }


    public virtual async ValueTask<long> GetLongCountAsync(CancellationToken cancellationToken = default)
    {

        return await _unitOfWork.Repository<TEntity>().CountAsync(cancellationToken);
    }

    public async ValueTask<IEnumerable<TDto>> FindAsync<TDto>(
   Expression<Func<TEntity, bool>> predicate,

   CancellationToken cancellationToken = default,
   IEnumerable<string>? includes = null) where TDto : class
    {
        var entities = await _unitOfWork.Repository<TEntity>().ListAsync(predicate, includes, cancellationToken);

        return _mapper.Map<IEnumerable<TDto>>(entities);
    }


    public async ValueTask<PaginatedList<TDto>> GetPaginatedAsync<TDto>(
        Expression<Func<TEntity, bool>> predicate,
        PaginationSearchModel paginationParams,

        CancellationToken cancellationToken = default,
        IEnumerable<string>? includes = null) where TDto : class
    {
        // Get paginated entities
        var paginatedEntities = await _unitOfWork.Repository<TEntity>().PaginateAsync(
            predicate,
            paginationParams.PageIndex,
            paginationParams.PageSize,
            includes,
            cancellationToken);

        return _mapper.Map<PaginatedList<TDto>>(paginatedEntities);

    }


    #endregion


    #region Command

    public virtual async ValueTask<Unit> CreateAsync<TDto>(TDto dto, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class
    {
        var mappedEntity = _mapper.Map<TEntity>(dto);

        _unitOfWork.Repository<TEntity>().Create(mappedEntity);

        if (autoSave)
            await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new();
    }
    public virtual async ValueTask<Unit> CreateRangeAsync<TDto>(IEnumerable<TDto> dtos, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class
    {

        var mappedEntites = dtos.Select(_mapper.Map<TEntity>);

        _unitOfWork.Repository<TEntity>().CreateRange(mappedEntites);
        if (autoSave)
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new();
    }

    public virtual async ValueTask<Unit> UpdateAsync<TDto>(
      TDto dto,
      bool autoSave = true,


      CancellationToken cancellationToken = default) where TDto : class
    {
        var entity = _mapper.Map<TEntity>(dto);

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

        CancellationToken cancellationToken = default) where TDto : class
    {
        var entitiesToUpdate = dtos.Select(_mapper.Map<TEntity>);

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


        CancellationToken cancellationToken = default) where TDto : class
    {
        var entity = _mapper.Map<TEntity>(dto);

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

    public virtual async Task<Unit> SoftDeleteByIdAsync(
        long id,
        bool autoSave = true,
        CancellationToken cancellationToken = default)
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

    public virtual async Task<Unit> SoftDeleteFirstAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = true,
        CancellationToken cancellationToken = default)
    {

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

    public virtual async Task<Unit> SoftDeleteRangeAsync(
       Expression<Func<TEntity, bool>> predicate,
       bool autoSave = true,
       CancellationToken cancellationToken = default)
    {
        // Ensure TEntity is a SoftDeletableEntity
        if (!typeof(SoftDeletableEntity<long>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException("Cannot perform soft delete on this entity type.");
        }

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
    public virtual async ValueTask<Unit> HardDeleteAsync<TDto>(TDto dto, bool autoSave = true, CancellationToken cancellationToken = default) where TDto : class
    {
        var entityToDelete = _mapper.Map<TEntity>(dto);

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

    public virtual async Task<Unit> HardDeleteByIdAsync(
        long id,
        bool autoSave = true,
        CancellationToken cancellationToken = default)
    {
        var entityToDelete = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id, null, cancellationToken);

        if (entityToDelete == null)
        {
            throw new NotFoundException(Resource.NotFoundInDB_Message);
        }

        _unitOfWork.Repository<TEntity>().HardDelete(entityToDelete);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new();
    }

    public virtual async Task<Unit> HardDeleteFirstAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = true,
        CancellationToken cancellationToken = default)
    {

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

    public virtual async Task<Unit> HardDeleteRangeAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = true,
        CancellationToken cancellationToken = default)
    {
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


    CancellationToken cancellationToken = default) where TDto : class
    {
        var entityToRecover = _mapper.Map<TEntity>(dto);

        if (entityToRecover is null)
        {
            throw new NotFoundException("Entity not found for recovery.");
        }

        if (entityToRecover is not SoftDeletableEntity<long> softDeletableEntity)
        {
            throw new InvalidOperationException("Entity does not support recovery.");
        }

        _unitOfWork.Repository<TEntity>().Recover(softDeletableEntity);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        // Return the mapped DTO
        return _mapper.Map<TDto>(entityToRecover);
    }

    public virtual async Task<Unit> RecoverByIdAsync(long id, bool autoSave = true, CancellationToken cancellationToken = default)
    {
        var entityToRecover = await _unitOfWork.Repository<TEntity>().GetByIdAsync(id, null, cancellationToken, true);

        if (entityToRecover is null)
        {
            throw new NotFoundException("Entity not found for recovery.");
        }


        if (entityToRecover is not SoftDeletableEntity<long> softDeletableEntity)
        {
            throw new InvalidOperationException("Entity does not support recovery.");
        }

        _unitOfWork.Repository<TEntity>().Recover(softDeletableEntity);

        if (autoSave)
        {
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }

        return new Unit();
    }


    public virtual async Task<Unit> RecoverRangeAsync(
        Expression<Func<TEntity, bool>> predicate,
        bool autoSave = true,
        CancellationToken cancellationToken = default)
    {
        if (!typeof(SoftDeletableEntity<long>).IsAssignableFrom(typeof(TEntity)))
        {
            throw new InvalidOperationException("Cannot perform Recover  on this entity type.");
        }

        var entitiesToRecover = await _unitOfWork.Repository<TEntity>().ListAsync(predicate, null, cancellationToken, true);

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