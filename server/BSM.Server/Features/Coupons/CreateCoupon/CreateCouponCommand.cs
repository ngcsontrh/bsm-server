using BSM.Framework.Mediator.Abstractions;
using BSM.Server.Common.Models;

namespace BSM.Server.Features.Coupons.CreateCoupon;

public class CreateCouponCommand : IRequest<HandlerResult>
{
    public string? Code { get; set; }
    public string? Name { get; set; }
    public int? DiscountPercent { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string? Description { get; set; }
    public string? Status { get; set; }
}
