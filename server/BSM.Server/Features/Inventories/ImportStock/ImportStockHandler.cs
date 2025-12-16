using System.Text.RegularExpressions;
using BSM.Domain.Entities;
using BSM.Framework.Mediator.Abstractions;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Models;
using BSM.Server.Features.Inventories.Import;
using Microsoft.EntityFrameworkCore;

namespace BSM.Server.Features.Inventories.ImportStock;

public class ImportStockHandler : IRequestHandler<ImportStockCommand, HandlerResult>
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ImportStockHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HandlerResult> Handle(ImportStockCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        if (currentUser == null || !currentUser.IsInRole("ADMIN") || !currentUser.IsInRole("MANAGER"))
        {
            return HandlerResult.Failure("Bạn không có quyền thực hiện thao tác này");
        }

        // Validate mã phiếu nhập
        if (string.IsNullOrWhiteSpace(request.Code))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã phiếu nhập không được để trống." } }
            });

        if (request.Code.Length != 11)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã phiếu nhập phải có độ dài 11 ký tự." } }
            });

        if (!Regex.IsMatch(request.Code, @"^PN-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                {
                    nameof(request.Code),
                    new[] { "Mã phiếu nhập không đúng định dạng. Định dạng đúng là 'PN-XXXXXXXX'." }
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

        if (request.Quantity >= 1000)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Quantity), new[] { "Số lượng không được ." } }
            });

        // Validate ngày nhập
        if (request.ImportDate == null)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.ImportDate), new[] { "Ngày nhập không được để trống." } }
            });
        if (request.ImportDate > DateTime.Now)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.ImportDate), new[] { "Ngày nhập không được lớn hơn ngày hiện tại." } }
            });

        // Tính tổng tiền nhập kho
        var totalPrice = request.Quantity.Value * currentBook.Price;

        // Tạo phiếu nhập kho
        await _dbContext.StockImports.AddAsync(
            new StockImportEntity
            {
                Id = Guid.NewGuid(),
                Code = request.Code,
                BookId = currentBook.Id,
                Quantity = request.Quantity.Value,
                ImportDate = request.ImportDate.Value,
                TotalPrice = totalPrice
            }, cancellationToken);

        var currentBookInventory = await _dbContext.Inventories.Where(i => i.BookId == currentBook.Id)
            .FirstOrDefaultAsync(cancellationToken);

        // Nếu chua có tồn kho cho sách này, tạo mới
        currentBookInventory ??= new InventoryEntity
        {
            Id = Guid.CreateVersion7(), BookId = currentBook.Id, Quantity = 0
        };
        // Cập nhật số lượng tồn kho
        currentBookInventory.Quantity += request.Quantity.Value;

        // Commit thay đổi vào database
        await _dbContext.SaveChangesAsync(cancellationToken);
        return HandlerResult.Success();
    }
}
