using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.BookModule.ValueObjects;

public record MoneyObject
{
    public decimal Amount { get; init; }
    public string Currency { get; init; } = null!;
    
    private MoneyObject() { }

    private MoneyObject(decimal amount, string currency)
    {
        Amount = amount;
        Currency = currency;
    }

    public static MoneyObject Create(decimal value, string currency)
    {
        if (value < 0) throw new DomainException("Money cannot be negative");
        if (string.IsNullOrWhiteSpace(currency)) throw new DomainException("Currency cannot be empty");
        return new MoneyObject(value, currency);
    }
}