using System.Text.RegularExpressions;
using BSM.Framework.Mediator.Abstractions;
using BSM.Server.Common.Models;

namespace BSM.Server.Features.Orders.CreateOrder;

public class CreateOrderCommand : ICommand<HandlerResult>
{
    public string? OrderCode { get; set; }
    public string? CustomerCode { get; set; }
    public IReadOnlyCollection<BookItem> BookItems { get; set; } = [];
    public string? ShippingAddress { get; set; }
    public string? PhoneNumber { get; set; }
    public string? PaymentMethod { get; set; }
    public string? CouponCode { get; set; }
    public string? Status { get; set; }
}

public class BookItem
{
    public string? BookCode { get; set; }
    public int Quantity { get; set; }
}
