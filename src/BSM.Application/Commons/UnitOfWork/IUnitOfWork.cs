namespace BSM.Application.Commons.UnitOfWork;

public interface IUnitOfWork
{
    Task BeginAsync(CancellationToken token = default);
    Task CommitAsync(CancellationToken token = default);
    Task RollbackAsync(CancellationToken token = default);
}