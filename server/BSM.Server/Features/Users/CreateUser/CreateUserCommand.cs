using BSM.Framework.Mediator.Abstractions;
using BSM.Server.Common.Models;

namespace BSM.Server.Features.Users.CreateUser;

public class CreateUserCommand : IRequest<HandlerResult>
{
    public string? Email { get; set; }
    public string? PasswordHash { get; set; }
    public string? FullName { get; set; }
    public string? PhoneNumber { get; set; }
    public string? Role { get; set; }
}
