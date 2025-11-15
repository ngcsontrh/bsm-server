using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.IdentityModule.ValueObjects;

namespace BSM.Domain.Modules.IdentityModule.Entities;

public class UserInformationEntity : EntityBase
{
    public Guid AccountId { get; set; }
    public UniqueCodeObject Code { get; private set; }
    public string FullName { get; private set; }
    public DateOnly? BirthDate { get; private set; }
    public IdentityNumberObject? IdentityNumber { get; private set; }
    
    private UserInformationEntity() { }

    private UserInformationEntity(Guid accountId, UniqueCodeObject code, string fullName)
    {
        AccountId = accountId;
        Code = code;
        FullName = fullName;
    }

    public static UserInformationEntity Create(Guid accountId, UniqueCodeObject code, string fullName)
    {
        return new UserInformationEntity(accountId, code, fullName);
    }

    public void ChangeCode(UniqueCodeObject code)
    {
        Code = code;
    }

    public void ChangeFullName(string fullName)
    {
        FullName = fullName;
    }
    
    public void ChangeBirthDate(DateOnly? birthDate)
    {
        BirthDate = birthDate;
    }
    
    public void ChangeIdentityNumber(IdentityNumberObject identityNumber)
    {
        IdentityNumber = identityNumber;
    }
}