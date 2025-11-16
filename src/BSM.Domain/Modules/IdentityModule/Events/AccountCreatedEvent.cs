using BSM.Domain.Commons.Events;

namespace BSM.Domain.Modules.IdentityModule.Events;

public record AccountCreatedEvent(Guid AccountId, string UserName, string Email, DateTime TimeStamp) : DomainEvent(TimeStamp);