using MediatR;

namespace BSM.Domain.Commons.Events;

public class DomainEvent : INotification
{
    public DateTimeOffset TimeStamp { get; set; }
    
    protected DomainEvent(DateTimeOffset timeStamp)
    {
        TimeStamp = timeStamp;
    }
}