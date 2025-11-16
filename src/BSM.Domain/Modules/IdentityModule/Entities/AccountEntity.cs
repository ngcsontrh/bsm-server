using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BSM.Domain.Modules.IdentityModule.Entities;

public class AccountEntity : IdentityUser<Guid>, IAuditable, IAggregateRoot
{
    public DateTime CreatedAt { get; private set; }
    public Guid? CreatorId { get; private set; }
    public string? CreatorName { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }
    public Guid? LastModifierId { get; private set; }
    public string? LastModifierName { get; private set; }

    public UserInformationEntity? UserInformation { get; private set; }

    private AccountEntity() { }

    public static AccountEntity Create(string userName, string email)
    {
        ValidateUserName(userName);
        ValidateEmail(email);

        var account = new AccountEntity
        {
            Id = Guid.CreateVersion7(),
            UserName = userName,
            Email = email,
            NormalizedUserName = userName.ToUpperInvariant(),
            NormalizedEmail = email.ToUpperInvariant(),
            EmailConfirmed = false,
            PhoneNumberConfirmed = false,
            TwoFactorEnabled = false,
            LockoutEnabled = true,
            AccessFailedCount = 0,
            SecurityStamp = Guid.NewGuid().ToString(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        return account;
    }

    public void UpdateEmail(string email)
    {
        ValidateEmail(email);
        Email = email;
        NormalizedEmail = email.ToUpperInvariant();
        EmailConfirmed = false;
    }

    public void ConfirmEmail()
    {
        EmailConfirmed = true;
    }

    public void UpdatePhoneNumber(string? phoneNumber)
    {
        PhoneNumber = phoneNumber;
        PhoneNumberConfirmed = false;
    }

    public void ConfirmPhoneNumber()
    {
        PhoneNumberConfirmed = true;
    }

    public void EnableTwoFactor()
    {
        TwoFactorEnabled = true;
    }

    public void DisableTwoFactor()
    {
        TwoFactorEnabled = false;
    }

    public void Lock(DateTimeOffset? lockoutEnd = null)
    {
        LockoutEnd = lockoutEnd ?? DateTimeOffset.UtcNow.AddYears(100);
    }

    public void Unlock()
    {
        LockoutEnd = null;
        AccessFailedCount = 0;
    }

    public void SetUserInformation(string code, string fullName)
    {
        if (UserInformation != null)
            throw new DomainException("User information already exists");

        UserInformation = UserInformationEntity.Create(Id, code, fullName);
    }

    public void UpdateUserInformationCode(string code)
    {
        if (UserInformation == null)
            throw new DomainException("User information does not exist");

        UserInformation.UpdateCode(code);
    }

    public void UpdateUserInformationFullName(string fullName)
    {
        if (UserInformation == null)
            throw new DomainException("User information does not exist");

        UserInformation.UpdateFullName(fullName);
    }

    public void UpdateUserInformationBirthDate(DateOnly? birthDate)
    {
        if (UserInformation == null)
            throw new DomainException("User information does not exist");

        UserInformation.UpdateBirthDate(birthDate);
    }

    private static void ValidateUserName(string userName)
    {
        if (string.IsNullOrWhiteSpace(userName))
            throw new DomainException("UserName cannot be empty");

        if (userName.Length > 256)
            throw new DomainException("UserName cannot exceed 256 characters");
    }

    private static void ValidateEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            throw new DomainException("Email cannot be empty");

        if (email.Length > 256)
            throw new DomainException("Email cannot exceed 256 characters");
    }
}