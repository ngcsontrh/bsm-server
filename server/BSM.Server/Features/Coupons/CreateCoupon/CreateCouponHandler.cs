using BSM.Domain.Entities;
using BSM.Framework.Mediator.Abstractions;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace BSM.Server.Features.Coupons.CreateCoupon;

public class CreateCouponHandler : IRequestHandler<CreateCouponCommand, HandlerResult>
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCouponHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HandlerResult> Handle(CreateCouponCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        if (currentUser == null || !currentUser.IsInRole("ADMIN") || !currentUser.IsInRole("MANAGER"))
        {
            return HandlerResult.Failure("Bạn không có quyền thực hiện thao tác này");
        }

        // 1. Mã khuyến mãi
        if (string.IsNullOrWhiteSpace(request.Code))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã khuyến mãi không hợp lệ, phải đúng 6 ký tự" } }
            });

        if (request.Code.Length != 11)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã khuyến mãi không hợp lệ, phải đúng 11 ký tự" } }
            });

        if (!Regex.IsMatch(request.Code, @"^KM-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã sách sai định dạng (KM-xxxxxxxx)." } }
            });

        var isCodeExists = await _dbContext.Coupons.AnyAsync(v => v.Code == request.Code, cancellationToken);
        if (isCodeExists)
            return HandlerResult.Failure("Mã khuyến mãi đã tồn tại");

        // 2. Tên khuyến mãi
        if (string.IsNullOrWhiteSpace(request.Name))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Name), new[] { "Tên khuyến mãi không được để trống" } }
            });

        if (request.Name.Length > 100)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Name), new[] { "Tên khuyến mãi không hợp lệ, tối đa 100 ký tự" } }
            });

        // 3. Phần trăm giảm giá
        if (request.DiscountPercent == null)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.DiscountPercent), new[] { "Phần trăm giảm giá không được để trống" } }
            });

        if (request.DiscountPercent <= 0)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.DiscountPercent), new[] { "Phần trăm giảm giá phải lớn hơn 0" } }
            });

        if (request.DiscountPercent > 100)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.DiscountPercent), new[] { "Phần trăm giảm giá không được vượt quá 100%." } }
            });

        // 4. Ngày bắt đầu
        if (request.StartDate == null)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.StartDate), new[] { "Ngày bắt đầu không được để trống" } }
            });

        if (request.StartDate.Value.Date < DateTime.Now.Date)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.StartDate), new[] { "Ngày bắt đầu không được nhỏ hơn ngày hiện tại" } }
            });

        // 5. Ngày kết thúc
        if (request.EndDate == null)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.EndDate), new[] { "Ngày kết thúc không được để trống" } }
            });

        if (request.EndDate < request.StartDate)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.EndDate), new[] { "Ngày kết thúc phải lớn hơn hoặc bằng ngày bắt đầu" } }
            });

        // 6. Mô tả khuyến mãi
        if (string.IsNullOrWhiteSpace(request.Description))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Description), new[] { "Mô tả khuyến mãi không được để trống" } }
            });

        if (request.Description.Length > 255)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Description), new[] { "Mô tả khuyến mãi vượt quá 255 ký tự" } }
            });

        // 7. Trạng thái khuyến mãi
        if (string.IsNullOrWhiteSpace(request.Status))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Status), new[] { "Trạng thái khuyến mãi không được để trống" } }
            });

        if (request.Status != "ACTIVE" && request.Status != "INACTIVE")
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Status), new[] { "Trạng thái khuyến mãi không hợp lệ" } }
            });

        // Tạo mới khuyến mãi
        var newCoupon = new CouponEntity
        {
            Id = Guid.CreateVersion7(),
            Code = request.Code,
            Name = request.Name,
            DiscountAmount = request.DiscountPercent.Value,
            IssueDate = request.StartDate.Value,
            ExpiryDate = request.EndDate.Value,
            Description = request.Description,
            Status = request.Status
        };
        await _dbContext.Coupons.AddAsync(newCoupon, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return HandlerResult.Success();
    }
}
