using Domain.Entities;
using LanguageExt.Common;

namespace Application.Interfaces.Persistence;

public interface IUnitOfWork : IDisposable
{
    IRepository<T> Repository<T>() where T : BaseEntity;

    Result<int> SaveChanges();

    Task<Result<int>> SaveChangesAsync(CancellationToken cancellationToken = default);
}
