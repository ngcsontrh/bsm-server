using BSM.Domain.Commons.Contracts;

namespace BSM.Domain.Commons.Repositories;

public interface IArRepository<TEntity> where TEntity : class, IEntityBase, IAggregateRoot
{
    TEntity? GetById(Guid id, CancellationToken token = default);
    Task AddAsync(TEntity entity, CancellationToken token = default);
    Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default);
    Task UpdateAsync(TEntity entity, CancellationToken token = default);
    Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default);
    Task DeleteAsync(TEntity entity, CancellationToken token = default);
    Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default);
}