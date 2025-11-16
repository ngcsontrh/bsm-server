using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Repositories;
using BSM.Infrastructure.Persistent;
using Microsoft.EntityFrameworkCore;

namespace BSM.Infrastructure.ArRepositories;

public abstract class ArRepository<TEntity> : IArRepository<TEntity> where TEntity : class, IEntityBase, IAggregateRoot
{
    protected readonly AppDbContext _context;

    public ArRepository(AppDbContext context)
    {
        _context = context;
    }

    public virtual TEntity? GetById(Guid id, CancellationToken token = default)
    {
        return _context.Set<TEntity>().Find(new object[] { id }, token);
    }

    public virtual Task AddAsync(TEntity entity, CancellationToken token = default)
    {
        _context.Set<TEntity>().Add(entity);
        return Task.CompletedTask;
    }

    public virtual Task AddRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        _context.Set<TEntity>().AddRange(entities);
        return Task.CompletedTask;
    }

    public virtual Task UpdateAsync(TEntity entity, CancellationToken token = default)
    {
        var entry = _context.Entry(entity);
        
        if (entry.State == EntityState.Detached)
        {
            _context.Set<TEntity>().Update(entity);
        }
        
        return Task.CompletedTask;
    }

    public virtual Task UpdateRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        foreach (var entity in entities)
        {
            var entry = _context.Entry(entity);
            
            if (entry.State == EntityState.Detached)
            {
                _context.Set<TEntity>().Update(entity);
            }
        }
        
        return Task.CompletedTask;
    }

    public virtual Task DeleteAsync(TEntity entity, CancellationToken token = default)
    {
        _context.Set<TEntity>().Remove(entity);
        return Task.CompletedTask;
    }

    public virtual Task DeleteRangeAsync(IEnumerable<TEntity> entities, CancellationToken token = default)
    {
        _context.Set<TEntity>().RemoveRange(entities);
        return Task.CompletedTask;
    }
}