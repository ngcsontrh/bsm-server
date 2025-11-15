using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.IdentityModule.ValueObjects;
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
    
    public UserInformationEntity UserInformation { get; private set; } = null!;

    public void ChangeUserInformation(UniqueCodeObject code, string fullName, DateOnly? birthdate, IdentityNumberObject identityNumber)
    {
        UserInformation.ChangeCode(code);
        UserInformation.ChangeFullName(fullName);
        UserInformation.ChangeBirthDate(birthdate);
        UserInformation.ChangeIdentityNumber(identityNumber);
    }
}