using BSM.Framework.Mediator.Abstractions;
using BSM.Server.Common.Models;

namespace BSM.Server.Features.Cart.AddToCart;

public class AddToCartCommand : ICommand<HandlerResult>
{
    public string? BookCode { get; set; }
    public int? Quantity { get; set; }
    public string? CouponCode { get; set; }
}
