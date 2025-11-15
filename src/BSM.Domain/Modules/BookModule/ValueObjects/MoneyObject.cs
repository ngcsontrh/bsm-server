using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.BookModule.ValueObjects;

public record MoneyObject
{
    public decimal Amount { get; init; }
    public string CurrencyCode { get; init; }

    private  MoneyObject() { }
    
    private MoneyObject(decimal amount, string currencyCode)
    {
        Amount = amount;
        CurrencyCode = currencyCode;
    }
    
    public static MoneyObject Create(decimal amount, string currencyCode)
    {
        if (amount < 0) throw new DomainException("Amount cannot be negative");
        return new MoneyObject(amount, currencyCode);
    }
}