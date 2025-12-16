using BSM.Framework.Mediator.Abstractions;
using BSM.Server.Common.Models;

namespace BSM.Server.Features.Orders.CancelOrder;

public class CancelOrderCommand : ICommand<HandlerResult>
{
    public string? OrderCode { get; set; }
    public string? Reason { get; set; }
}
