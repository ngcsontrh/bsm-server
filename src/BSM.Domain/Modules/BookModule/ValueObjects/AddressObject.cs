using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.BookModule.ValueObjects;

public record AddressObject
{
    public string Street { get; init; } = null!;
    public string District { get; init; } = null!;
    public string Province { get; init; } = null!;
    public string Country { get; init; } = null!;
    
    private AddressObject() { }
    
    private AddressObject(string street, string district, string province, string country)
    {
        Street = street;
        District = district;
        Province = province;
        Country = country;
    }

    public static AddressObject Create(string street, string district, string province, string country)
    {
        if (string.IsNullOrEmpty(street)) throw new DomainException("Street cannot be null or empty");
        if (string.IsNullOrEmpty(district)) throw new DomainException("District cannot be null or empty");
        if (string.IsNullOrEmpty(province)) throw new DomainException("Province cannot be null or empty");
        if (string.IsNullOrEmpty(country)) throw new DomainException("Country cannot be null or empty");
        return new AddressObject(street, district, province, country);
    }
}