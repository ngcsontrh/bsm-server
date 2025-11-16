using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.BookModule.Entities;

public class CategoryEntity : EntityBase, IAggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? Description { get; private set; }

    private CategoryEntity() { }

    public static CategoryEntity Create(string name)
    {
        ValidateName(name);

        var category = new CategoryEntity
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow,
            Name = name
        };

        return category;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Category name cannot be empty");

        if (name.Length > 255)
            throw new DomainException("Category name cannot exceed 255 characters");
    }
}