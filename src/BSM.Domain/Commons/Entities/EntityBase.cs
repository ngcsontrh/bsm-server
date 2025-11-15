using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Events;

namespace BSM.Domain.Commons.Entities;

public class EntityBase : IEntityBase, IAuditable
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public Guid? CreatorId { get; protected set; }
    public string? CreatorName { get; protected set; }
    public DateTime? LastModifiedAt { get; protected set; }
    public Guid? LastModifierId { get; protected set; }
    public string? LastModifierName { get; protected set; }
    
    private readonly List<DomainEvent> _domainEvents = new();
    public IReadOnlyCollection<DomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    public void AddDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void RemoveDomainEvent(DomainEvent domainEvent)
    {
        _domainEvents.Remove(domainEvent);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}