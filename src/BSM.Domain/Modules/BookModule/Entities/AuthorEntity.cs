using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.BookModule.ValueObjects;

namespace BSM.Domain.Modules.BookModule.Entities;

public class AuthorEntity : EntityBase, IAggregateRoot
{
    public string Name { get; private set; } = null!;
    public string? Biography { get; private set; }
    public ImageObject? Image { get; private set; }

    private AuthorEntity() { }

    public static AuthorEntity Create(string name)
    {
        ValidateName(name);

        var author = new AuthorEntity
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow,
            Name = name
        };

        return author;
    }

    public void UpdateBiography(string? biography)
    {
        Biography = biography;
    }

    public void UpdateImage(ImageObject? image)
    {
        Image = image;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Author name cannot be empty");

        if (name.Length > 255)
            throw new DomainException("Author name cannot exceed 255 characters");
    }
}