using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.BookModule.ValueObjects;

namespace BSM.Domain.Modules.BookModule.Entities;

public class PublisherEntity : EntityBase, IAggregateRoot
{
    public string Name { get; private set; } = null!;
    public AddressObject? Address { get; private set; }

    private PublisherEntity() { }

    public static PublisherEntity Create(string name)
    {
        ValidateName(name);

        var publisher = new PublisherEntity
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow,
            Name = name
        };

        return publisher;
    }

    public void UpdateAddress(AddressObject? address)
    {
        Address = address;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Publisher name cannot be empty");

        if (name.Length > 255)
            throw new DomainException("Publisher name cannot exceed 255 characters");
    }
}