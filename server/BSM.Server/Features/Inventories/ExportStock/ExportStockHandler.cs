using System.Text.RegularExpressions;
using BSM.Domain.Entities;
using BSM.Framework.Mediator.Abstractions;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BSM.Server.Features.Inventories.ExportStock;

public class ExportStockHandler : IRequestHandler<ExportStockCommand, HandlerResult>
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ExportStockHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HandlerResult> Handle(ExportStockCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        if (currentUser == null || (!currentUser.IsInRole("ADMIN") && !currentUser.IsInRole("MANAGER")))
        {
            return HandlerResult.Failure("Bạn không có quyền thực hiện thao tác này");
        }

        // Validate mã phiếu xuất
        if (string.IsNullOrWhiteSpace(request.Code))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã phiếu xuất không được để trống." } }
            });

        if (request.Code.Length != 11)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã phiếu xuất phải có độ dài 11 ký tự." } }
            });

        if (!Regex.IsMatch(request.Code, @"^PX-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                {
                    nameof(request.Code),
                    new[] { "Mã phiếu xuất không đúng định dạng. Định dạng đúng là 'PX-XXXXXXXX'." }
                }
            });

        // Validate mã sách
        if (string.IsNullOrWhiteSpace(request.BookCode))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.BookCode), new[] { "Mã sách không được để trống." } }
            });

        if (request.BookCode.Length != 10)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.BookCode), new[] { "Mã sách phải có độ dài 10 ký tự." } }
            });

        if (!Regex.IsMatch(request.BookCode, @"^S-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                {
                    nameof(request.BookCode), new[] { "Mã sách không đúng định dạng. Định dạng đúng là 'S-XXXXXXXX'." }
                }
            });

        var currentBook =
            await _dbContext.Books.FirstOrDefaultAsync(b => b.Code == request.BookCode, cancellationToken);
        if (currentBook == null)
            return HandlerResult.Failure("Sách không tồn tại trong hệ thống");

        // Validate số lượng
        if (request.Quantity == null)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Quantity), new[] { "Số lượng không được để trống." } }
            });

        if (request.Quantity <= 0)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Quantity), new[] { "Số lượng phải lớn hơn 0." } }
            });

        var currentBookInventory = await _dbContext.Inventories.Where(i => i.BookId == currentBook.Id)
            .FirstAsync(cancellationToken);
        if (request.Quantity >= currentBookInventory.Quantity)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Quantity), new[] { "Số lượng không được lớn hơn số lượng tồn kho." } }
            });

        // Validate ngày xuất
        if (request.ExportDate == null)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.ExportDate), new[] { "Ngày xuất không được để trống." } }
            });
        if (request.ExportDate > DateTime.Now)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.ExportDate), new[] { "Ngày xuất không được lớn hơn ngày hiện tại." } }
            });

        // Tính tổng tiền
        var totalPrice = request.Quantity.Value * currentBook.Price;
        // Tạo phiếu xuất kho mới
        await _dbContext.StockExports.AddAsync(
            new StockExportEntity
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                BookId = currentBook.Id,
                Quantity = request.Quantity.Value,
                ExportDate = request.ExportDate.Value,
                TotalPrice = totalPrice
            }, cancellationToken);

        // Cập nhật lại số lượng tồn kho
        currentBookInventory.Quantity -= request.Quantity.Value;

        // Lưu thay đổi vào database
        await _dbContext.SaveChangesAsync(cancellationToken);
        return HandlerResult.Success();
    }
}
