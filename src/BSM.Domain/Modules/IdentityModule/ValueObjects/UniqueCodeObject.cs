using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.IdentityModule.ValueObjects;

public record UniqueCodeObject
{
    public string Value { get; init; }
    
    private UniqueCodeObject()
    {
    }

    private UniqueCodeObject(string value)
    {
        Value = value;
    }

    public static UniqueCodeObject Create(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length > 20) throw new DomainException("Code is invalid");
        return new UniqueCodeObject(value);
    }
}