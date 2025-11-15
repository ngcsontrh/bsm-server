using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.IdentityModule.ValueObjects;

public record IdentityNumberObject
{
    public string Value { get; }
    
    private IdentityNumberObject() { }

    private IdentityNumberObject(string value)
    {
        Value = value;
    }

    public static IdentityNumberObject Create(string value)
    {
        if (string.IsNullOrEmpty(value) || !value.All(char.IsDigit) || value.Length != 12) throw new DomainException("Identity number is invalid");
        return new IdentityNumberObject(value);
    }
}