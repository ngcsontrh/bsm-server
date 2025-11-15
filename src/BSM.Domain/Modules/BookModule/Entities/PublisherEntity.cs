using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Modules.BookModule.ValueObjects;

namespace BSM.Domain.Modules.BookModule.Entities;

public class PublisherEntity : EntityBase, IAggregateRoot
{
    public string Name { get; private set; }
    public AddressObject Address { get; private set; }

    private PublisherEntity() { }
    
    private PublisherEntity(string name, AddressObject address)
    {
        Name = name;
        Address = address;
    }

    public static PublisherEntity Create(string name, AddressObject address)
    {
        return new PublisherEntity(name, address);
    }

    public void ChangeName(string name)
    {
        Name = name;
    }
    
    public void ChangeAddress(AddressObject address)
    {
        Address = address;
    }
}