namespace BSM.Domain.Commons.Contracts;

public interface IEntityBase : IEntity
{
    Guid Id { get; }
}