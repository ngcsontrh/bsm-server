using System.Text.RegularExpressions;
using BSM.Framework.Mediator.Abstractions;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BSM.Server.Features.Orders.CancelOrder;

public class CancelOrderHandler : IRequestHandler<CancelOrderCommand, HandlerResult>
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CancelOrderHandler(AppDbContext context, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = context;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HandlerResult> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        if (currentUser == null || !currentUser.Identity!.IsAuthenticated)
        {
            return HandlerResult.Failure("Vui lòng đăng nhập để thực hiện thao tác này");
        }

        if (string.IsNullOrWhiteSpace(request.OrderCode))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.OrderCode), new[] { "Mã đơn hàng không được để trống." } }
            });

        if (request.OrderCode.Length != 12)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.OrderCode), new[] { "Mã đơn hàng phải có độ dài 11 ký tự." } }
            });

        if (!Regex.IsMatch(request.OrderCode, @"^ORD-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                {
                    nameof(request.OrderCode),
                    new[] { "Mã đơn hàng không đúng định dạng. Định dạng đúng là 'ORD-XXXXXXXX'." }
                }
            });
        var currentOrder = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Code == request.OrderCode, cancellationToken);
        if (currentOrder == null)
            return HandlerResult.Failure("Đơn hàng không tồn tại trong hệ thống");

        var validStatuses = new List<string> { "PENDING", "PROCESSING" };
        if (!validStatuses.Contains(currentOrder.Status))
            return HandlerResult.Failure("Trạng thái đơn hàng không hợp lệ");

        var currentUserId = currentUser.FindFirst("CustomerId")?.Value;
        var currentUserRole = currentUser.FindFirst("Role")?.Value;
        if (!(currentOrder.UserId == Guid.Parse(currentUserId!) || currentUserRole == "ADMIN" || currentUserRole == "MANAGER"))        
            return HandlerResult.Failure("Bạn không có quyền hủy đơn hàng này");
        currentOrder.Status = "CANCELLED";
        await _dbContext.SaveChangesAsync(cancellationToken);
        return HandlerResult.Success();
    }
}
