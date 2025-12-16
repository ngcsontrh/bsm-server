using System.Text.RegularExpressions;
using BSM.Domain.Entities;
using BSM.Framework.Mediator.Abstractions;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BSM.Server.Features.Orders.CreateOrder;

public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, HandlerResult>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;

    public CreateOrderHandler(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<HandlerResult> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        if (currentUser == null || !currentUser.Identity!.IsAuthenticated)
        {
            return HandlerResult.Failure("Vui lòng đăng nhập để thực hiện thao tác này");
        }

        // Validate mã đơn hàng
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
        if (request.OrderCode != "PENDING")
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.OrderCode), new[] { "Mã đơn hàng chỉ có thể là 'PENDING' khi tạo mới." } }
            });
        var isOrderCodeExists = await _dbContext.Orders
            .AnyAsync(o => o.Code == request.OrderCode, cancellationToken);
        if (isOrderCodeExists)
            return HandlerResult.Failure("Mã đơn hàng đã tồn tại trong hệ thống.");

        // Validate mã khách hàng
        if (string.IsNullOrWhiteSpace(request.CustomerCode))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.CustomerCode), new[] { "Mã khách hàng không được để trống." } }
            });
        if (request.CustomerCode.Length != 12)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.CustomerCode), new[] { "Mã khách hàng phải có độ dài 12 ký tự." } }
            });
        if (!Regex.IsMatch(request.CustomerCode, @"^CUS-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                {
                    nameof(request.CustomerCode),
                    new[] { "Mã khách hàng không đúng định dạng. Định dạng đúng là 'CUS-XXXXXXXX'." }
                }
            });
        var customer = await _dbContext.Customers
            .FirstOrDefaultAsync(c => c.Code == request.CustomerCode, cancellationToken);
        if (customer == null)
            return HandlerResult.Failure("Mã khách hàng không tồn tại trong hệ thống.");

        // Validate danh sách sản phẩm trong giỏ
        if (request.BookItems.Count <= 0)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.BookItems), new[] { "Danh sách sản phẩm trong giỏ không được để trống." } }
            });

        var isBookCodeValid = request.BookItems.Any(item => string.IsNullOrWhiteSpace(item.BookCode) ||
            item.BookCode.Length != 12 ||
            !Regex.IsMatch(item.BookCode, @"^S-[A-Za-z0-9]{8}$"));
        if (!isBookCodeValid)
            return HandlerResult.Failure("Danh sách sản phẩm trong giỏ có mã sách đúng định dạng trong hệ thống.");

        var bookCodes = request.BookItems.Select(item => item.BookCode).ToList();
        var inventoryBook = await _dbContext.Inventories
            .Where(i => bookCodes.Contains(i.Book.Code))
            .Include(i => i.Book)
            .ToListAsync(cancellationToken);
        var isBookQuantityValid = inventoryBook.Any(i =>
        {
            var requestedItem = request.BookItems.FirstOrDefault(item => item.BookCode == i.Book.Code);
            return requestedItem != null && requestedItem.Quantity > i.Quantity;
        });
        if (!isBookQuantityValid)
            return HandlerResult.Failure("Số lượng sản phẩm trong giỏ vượt quá số lượng tồn kho.");

        // Validate địa chỉ giao hàng
        if (string.IsNullOrWhiteSpace(request.ShippingAddress))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.ShippingAddress), new[] { "Địa chỉ giao hàng không được để trống." } }
            });

        if (request.ShippingAddress.Length < 5)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.ShippingAddress), new[] { "Địa chỉ giao hàng phải có độ dài ít nhất 5 ký tự." } }
            });

        if (request.ShippingAddress.Length > 200)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.ShippingAddress), new[] { "Địa chỉ giao hàng phải có độ dài ít nhất 5 ký tự." } }
            });

        // Validate số điện thoại
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PhoneNumber), new[] { "Số điện thoại không được để trống." } }
            });

        if (request.PhoneNumber.Length != 11)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PhoneNumber), new[] { "Số điện thoại phải có độ dài 11 ký tự." } }
            });

        // Validate phương thức thanh toán
        if (string.IsNullOrWhiteSpace(request.PaymentMethod))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PaymentMethod), new[] { "Phương thức thanh toán không được để trống." } }
            });
        var validPaymentMethods = new[] { "CASH", "QR" };
        if (!validPaymentMethods.Contains(request.PaymentMethod))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PaymentMethod), new[] { "Phương thức thanh toán không hợp lệ." } }
            });

        // Validate mã khuyến mãi
        if (string.IsNullOrWhiteSpace(request.CouponCode))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.CouponCode), new[] { "Mã khuyến mãi không hợp lệ, phải đúng 6 ký tự" } }
            });

        if (request.CouponCode.Length != 6)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.CouponCode), new[] { "Mã khuyến mãi không hợp lệ, phải đúng 6 ký tự" } }
            });

        if (!Regex.IsMatch(request.CouponCode, @"^KM-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.CouponCode), new[] { "Mã khuyến mãi sai định dạng (KM-xxxxxxxx)." } }
            });

        var currentCoupon = await _dbContext.Coupons
            .FirstOrDefaultAsync(c => c.Code == request.CouponCode, cancellationToken);
        if (currentCoupon == null)
            return HandlerResult.Failure("Mã khuyến mãi không tồn tại trong hệ thống.");

        if (currentCoupon.ExpiryDate < DateTime.Now)
            return HandlerResult.Failure("Mã khuyến mãi đã hết hạn.");

        if (currentCoupon.Status != "ACTIVE")
            return HandlerResult.Failure("Mã khuyến mãi chưa kích hoạt.");

        if (currentCoupon.IsUsed)
            return HandlerResult.Failure("Mã khuyến mãi đã được sử dụng.");

        // Tính tổng tiền đơn hàng
        var totalBookPrice = request.BookItems.Sum(item =>
        {
            var inventory = inventoryBook.FirstOrDefault(i => i.Book.Code == item.BookCode);
            return inventory != null ? inventory.Book.Price * item.Quantity : 0;
        });

        // Tạo đơn hàng mới
        var order = new OrderEntity
        {
            Id = Guid.CreateVersion7(),
            Code = request.OrderCode,
            OrderDate = DateTime.Now,
            Status = "PENDING",
            UserId = customer.Id,
            TotalAmount = totalBookPrice * currentCoupon.DiscountAmount / 100,
        };
        await _dbContext.Orders.AddAsync(order, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return HandlerResult.Success();
    }
}
