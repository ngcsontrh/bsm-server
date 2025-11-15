using BSM.Domain.Commons.Contracts;

namespace BSM.Domain.Commons.Repositories;

public interface IRepositoryBase<TEntity> where TEntity : class, IEntity, IAggregateRoot
{
    TEntity GetById(Guid id, CancellationToken token = default(CancellationToken));
    Task AddAsync(TEntity entity,  CancellationToken token = default(CancellationToken));
    Task AddRangeAsync(IEnumerable<TEntity> entities,  CancellationToken token = default(CancellationToken));
    Task UpdateAsync(TEntity entity, CancellationToken token = default(CancellationToken));
    Task UpdateRangeAsync(IEnumerable<TEntity> entities,  CancellationToken token = default(CancellationToken));
    Task DeleteAsync(TEntity entity,  CancellationToken token = default(CancellationToken));
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default(CancellationToken));
    Task<int> SaveChangesAsync(CancellationToken token = default(CancellationToken));
}