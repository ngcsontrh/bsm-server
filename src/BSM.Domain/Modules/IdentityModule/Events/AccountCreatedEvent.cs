using BSM.Domain.Commons.Events;

namespace BSM.Domain.Modules.IdentityModule.Events;

public class AccountCreatedEvent(Guid accountId, string userName, string email, DateTimeOffset timeStamp) : DomainEvent(timeStamp)
{
    public Guid AccountId { get; init; } = accountId;
    public string UserName { get; init; } = userName;
    public string Email { get; init; } = email;
}