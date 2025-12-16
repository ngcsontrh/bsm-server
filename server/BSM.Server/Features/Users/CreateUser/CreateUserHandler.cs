using System.Text.RegularExpressions;
using BSM.Domain.Entities;
using BSM.Framework.Mediator.Abstractions;
using BSM.Framework.PasswordHasher;
using BSM.Infrastructure.Persistence;
using BSM.Server.Common.Models;
using Microsoft.EntityFrameworkCore;

namespace BSM.Server.Features.Users.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, HandlerResult>
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher _passwordHasher;

    public CreateUserHandler(IHttpContextAccessor httpContextAccessor, AppDbContext dbContext,
        IPasswordHasher passwordHasher)
    {
        _httpContextAccessor = httpContextAccessor;
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<HandlerResult> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var currentUser = _httpContextAccessor.HttpContext?.User;
        if (currentUser == null || !currentUser.IsInRole("ADMIN"))
        {
            return HandlerResult.Failure("Bạn không có quyền tạo người dùng");
        }

        // Validate Email
        if (string.IsNullOrWhiteSpace(request.Email))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Email), new[] { "Email không được để trống" } }
            });

        var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(request.Email, emailRegex) || request.Email.Any(c => "#%&*^(),/".Contains(c)))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Email), new[] { "Email không đúng định dạng" } }
            });

        var isEmailExist = await _dbContext.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
        if (isEmailExist)
            return HandlerResult.Failure("Email đã được sử dụng");

        // Validate mật khẩu
        if (string.IsNullOrWhiteSpace(request.PasswordHash))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PasswordHash), new[] { "Mật khẩu không được để trống" } }
            });

        if (request.PasswordHash.Length < 6)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PasswordHash), new[] { "Mật khẩu phải có ít nhất 6 ký tự" } }
            });

        if (request.PasswordHash.Any(char.IsWhiteSpace))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PasswordHash), new[] { "Mật khẩu không được chứa khoảng trắng" } }
            });

        // Validate họ và tên
        if (string.IsNullOrWhiteSpace(request.FullName))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.FullName), new[] { "Họ tên không được để trống" } }
            });

        if (request.FullName.Length < 2)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.FullName), new[] { "Họ tên phải có ít nhất 2 ký tự" } }
            });

        if (!Regex.IsMatch(request.FullName, @"^[\p{L}\s]+$"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.FullName), new[] { "Họ tên chỉ được chứa ký tự chữ cái" } }
            });

        // Validate số điện thoại
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PhoneNumber), new[] { "Số điện thoại không được để trống" } }
            });

        if (!request.PhoneNumber.All(char.IsDigit))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PhoneNumber), new[] { "Số điện thoại chỉ được chứa chữ số" } }
            });

        if (request.PhoneNumber.Length != 10)
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PhoneNumber), new[] { "Số điện thoại phải có độ dài 10 chữ số" } }
            });

        if (!request.PhoneNumber.StartsWith("0"))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.PhoneNumber), new[] { "Số điện thoại phải bắt đầu bằng số 0" } }
            });

        var isPhoneExist =
            await _dbContext.Users.AnyAsync(u => u.PhoneNumber == request.PhoneNumber, cancellationToken);
        if (isPhoneExist)
            return HandlerResult.Failure("Số điện thoại đã được sử dụng");

        // Validate vai trò (Role)
        if (string.IsNullOrWhiteSpace(request.Role))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Role), new[] { "Vai trò không được để trống" } }
            });

        var validRoles = new[] { "MANAGER", "STAFF", "USER" };
        if (!validRoles.Contains(request.Role))
            return HandlerResult.ValidationFailure(new Dictionary<string, string[]>
            {
                { nameof(request.Role), new[] { "Vai trò không hợp lệ" } }
            });

        // Tạo người dùng mới
        var passwordHash = _passwordHasher.GenerateHash(request.PasswordHash);
        var newUser = new UserEntity
        {
            Id = Guid.CreateVersion7(),
            Email = request.Email,
            PasswordHash = passwordHash,
            FullName = request.FullName,
            PhoneNumber = request.PhoneNumber,
            Role = request.Role
        };
        await _dbContext.Users.AddAsync(newUser, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return HandlerResult.Success();
    }
}
