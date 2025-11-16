using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.IdentityModule.Entities;

public class UserInformationEntity : EntityBase
{
    public Guid AccountId { get; private set; }
    public string Code { get; private set; } = null!;
    public string FullName { get; private set; } = null!;
    public DateOnly? BirthDate { get; private set; }

    private UserInformationEntity() { }

    internal static UserInformationEntity Create(Guid accountId, string code, string fullName)
    {
        if (accountId == Guid.Empty)
            throw new DomainException("AccountId cannot be empty");

        ValidateCode(code);
        ValidateFullName(fullName);

        var userInfo = new UserInformationEntity
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow,
            AccountId = accountId,
            Code = code,
            FullName = fullName
        };

        return userInfo;
    }

    internal void UpdateCode(string code)
    {
        ValidateCode(code);
        Code = code;
    }

    internal void UpdateFullName(string fullName)
    {
        ValidateFullName(fullName);
        FullName = fullName;
    }

    internal void UpdateBirthDate(DateOnly? birthDate)
    {
        BirthDate = birthDate;
    }

    private static void ValidateCode(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("Code cannot be empty");

        if (code.Length > 50)
            throw new DomainException("Code cannot exceed 50 characters");
    }

    private static void ValidateFullName(string fullName)
    {
        if (string.IsNullOrWhiteSpace(fullName))
            throw new DomainException("FullName cannot be empty");

        if (fullName.Length > 255)
            throw new DomainException("FullName cannot exceed 255 characters");
    }
}