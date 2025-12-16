using System.Text.RegularExpressions;
using BSM.Domain.Entities;
using BSM.Framework.Mediator.Abstractions;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BSM.Server.Features.Books.CreateBook;

public class CreateBookHandler : IRequestHandler<CreateBookCommand, HandlerResult>
{
    private readonly AppDbContext _dbContext;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateBookHandler(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
    {
        _dbContext = dbContext;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<HandlerResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        if (currentUser == null || !currentUser.IsInRole("ADMIN") || !currentUser.IsInRole("MANAGER"))
        {
            return HandlerResult.Failure("Bạn không có quyền thực hiện thao tác này");
        }
        
        // Validate mã sách
        if (string.IsNullOrWhiteSpace(request.Code))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã sách không được để trống" } }
            });

        if (request.Code.Length != 10)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã sách không hợp lệ, độ dài phải là 10 ký tự" } }
            });

        if (!Regex.IsMatch(request.Code, @"^S-[A-Za-z0-9]{8}$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Code), new[] { "Mã sách sai định dạng (S-xxxxxxxx)" } }
            });

        var isBookExist = await _dbContext.Books
            .AnyAsync(b => b.Code == request.Code, cancellationToken);
        if (isBookExist)
            return HandlerResult.Failure("Mã sách đã tồn tại trong hệ thống");

        // Validate tên sách
        if (string.IsNullOrWhiteSpace(request.Name))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Name), new[] { "Tên sách không hợp lệ, phải có ít nhất 3 ký tự" } }
            });

        if (request.Name.Length < 1)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Name), new[] { "Tên sách không hợp lệ, phải có ít nhất 1 ký tự" } }
            });

        // Validate tên tác giả
        if (string.IsNullOrWhiteSpace(request.Author))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Author), new[] { "Tên tác giả không hợp lệ" } }
            });

        if (!Regex.IsMatch(request.Author, @"^[\p{L}\s]+$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Author), new[] { "Tên tác giả không hợp lệ" } }
            });

        if (request.Author.Length < 4)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Author), new[] { "Tên tác giả không thể ngắn hơn 4 ký tự" } }
            });

        // Validate giá tiền
        if (request.Price == null)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Price), new[] { "Không được để trống giá tiền" } }
            });

        if (request.Price < 0)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Price), new[] { "Giá tiền không thể âm! Yêu cầu nhập lại" } }
            });

        if (request.Price > 500000)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Price), new[] { "Giá tiền vượt quá quy định" } }
            });

        // Validate nhà xuất bản
        if (string.IsNullOrWhiteSpace(request.Publisher))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Publisher), new[] { "Không được để trống nhà xuất bản" } }
            });

        if (!Regex.IsMatch(request.Publisher, @"^[\p{L}\d\s]+$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Publisher), new[] { "Tên nhà xuất bản không hợp lệ" } }
            });

        if (request.Publisher.Length < 4)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Publisher), new[] { "Tên nhà xuất bản không hợp lệ" } }
            });

        // Tạo sách mới
        var newBook = new BookEntity
        {
            Id = Guid.CreateVersion7(),
            Code = request.Code,
            Author = request.Author,
            Name = request.Name,
            Publisher = request.Publisher,
            Price = request.Price.Value
        };
        await _dbContext.Books.AddAsync(newBook, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return HandlerResult.Success();
    }
}
