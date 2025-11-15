using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.BookModule.ValueObjects;

public record ISBNObject
{
    public string Value { get; init; }
    
    private ISBNObject() { }
    
    private ISBNObject(string value)
    {
        Value = value;
    }

    public static ISBNObject Create(string value)
    {
        if (value.Length > 13) throw new DomainException("ISBN length must be 13 characters or less");
        return new ISBNObject(value);
    }
}