using BSM.Application.Commons.UnitOfWork;
using Microsoft.EntityFrameworkCore.Storage;

namespace BSM.Infrastructure.Persistent;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;
    private IDbContextTransaction? _transaction;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }

    public async Task BeginAsync(CancellationToken token = default)
    {
        _transaction = await _context.Database.BeginTransactionAsync(token);
    }

    public async Task CommitAsync(CancellationToken token = default)
    {
        try
        {
            await _context.SaveChangesAsync(token);
            
            if (_transaction != null)
            {
                await _transaction.CommitAsync(token);
            }
        }
        catch
        {
            await RollbackAsync(token);
            throw;
        }
    }

    public async Task RollbackAsync(CancellationToken token = default)
    {
        try
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync(token);
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }
        catch
        {
            
        }
    }
}
