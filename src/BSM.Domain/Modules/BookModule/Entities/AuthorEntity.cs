using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.BookModule.ValueObjects;

namespace BSM.Domain.Modules.BookModule.Entities;

public class AuthorEntity : EntityBase, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Biography { get; private set; }
    public ImageObject? Image { get; private set; }
    
    private AuthorEntity() { }

    private AuthorEntity(string name, string biography, ImageObject? image)
    {
        Name = name;
        Biography = biography;
        Image = image;
    }

    public void ChangeName(string name)
    {
        if (string.IsNullOrEmpty(name) || name.Length > 100) throw new DomainException("Name is invalid");
        Name = name;
    }

    public void ChangeBiography(string? biography)
    {
        Biography = biography;
    }

    public void ChangeImage(ImageObject? image)
    {
        Image = image;
    }

    public static AuthorEntity Create(string name, string biography, ImageObject? image)
    {
        return new AuthorEntity(name, biography, image);
    }
}