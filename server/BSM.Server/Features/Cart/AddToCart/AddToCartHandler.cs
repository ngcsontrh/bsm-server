using System.Text.RegularExpressions;
using BSM.Domain.Entities;
using BSM.Framework.Mediator.Abstractions;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BSM.Server.Features.Cart.AddToCart;

public class AddToCartHandler : IRequestHandler<AddToCartCommand, HandlerResult>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;

    public AddToCartHandler(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
    }

    public async Task<HandlerResult> Handle(AddToCartCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        if (currentUser == null || !currentUser.Identity!.IsAuthenticated)
        {
            return HandlerResult.Failure("Vui lòng đăng nhập để thực hiện thao tác này");
        }

        // Validate mã sách
        if (string.IsNullOrWhiteSpace(request.BookCode))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.BookCode), new[] { "Mã sách không được để trống" } }
            });

        if (request.BookCode.Length != 10)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.BookCode), new[] { "Mã sách không hợp lệ, độ dài phải là 10 ký tự" } }
            });

        if (!Regex.IsMatch(request.BookCode, @"^S-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.BookCode), new[] { "Mã sách sai định dạng (S-xxxxxxxx)" } }
            });

        var currentBook =
            await _dbContext.Books.FirstOrDefaultAsync(b => b.Code == request.BookCode, cancellationToken);
        if (currentBook == null)
            return HandlerResult.Failure("Sách không tồn tại trong hệ thống");

        // Validate số lượng
        if (request.Quantity == null)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Quantity), new[] { "Số lượng không được để trống" } }
            });

        if (request.Quantity < 1)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Quantity), new[] { "Số lượng phải lớn hơn 0" } }
            });

        if (request.Quantity > 100)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Quantity), new[] { "Số lượng không được vượt quá 100" } }
            });

        // Validate mã giảm giá
        if (string.IsNullOrWhiteSpace(request.CouponCode))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.CouponCode), new[] { "Mã khuyến mãi không được để trống" } }
            });

        if (request.CouponCode.Length != 11)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.CouponCode), new[] { "Mã khuyến mãi không hợp lệ, độ dài phải là 11 ký tự" } }
            });

        if (!Regex.IsMatch(request.CouponCode, @"^KM-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.CouponCode), new[] { "Mã khuyến mãi sai định dạng (KM-xxxxxxxx)" } }
            });

        var currentCoupon =
            await _dbContext.Coupons.FirstOrDefaultAsync(c => c.Code == request.CouponCode, cancellationToken);
        if (currentCoupon == null)
            return HandlerResult.Failure("Mã khuyến mãi không tồn tại trong hệ thống");
        if (currentCoupon.ExpiryDate < DateTime.Now)
            return HandlerResult.Failure("Mã khuyến mãi đã hết hạn");
        if (currentCoupon.Status != "ACTIVE")
            return HandlerResult.Failure("Mã khuyến mãi không hợp lệ");

        // Thêm vào giỏ hàng
        await _dbContext.CartItems.AddAsync(
            new CartItemEntity
            {
                Id = Guid.CreateVersion7(),
                BookId = currentBook.Id,
                Quantity = request.Quantity.Value,
                CouponId = currentCoupon.Id
            }, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return HandlerResult.Success();
    }
}
