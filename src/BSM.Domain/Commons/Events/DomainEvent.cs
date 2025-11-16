using MediatR;

namespace BSM.Domain.Commons.Events;

public abstract record DomainEvent(DateTime TimeStamp) : INotification;