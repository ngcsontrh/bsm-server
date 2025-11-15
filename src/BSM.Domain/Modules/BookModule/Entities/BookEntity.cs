using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.BookModule.ValueObjects;

namespace BSM.Domain.Modules.BookModule.Entities;

public class BookEntity : EntityBase, IAggregateRoot
{
    public ISBNObject ISBN { get; private set; }
    public string Title { get; private set; }
    public string? Description { get; private set; }
    public int? PublicationYear { get; private set; }
    public ImageObject? CoverImage { get; private set; }
    public int? Quantity { get; private set; }
    public MoneyObject? Price { get; private set; }
    public Guid? PublisherId { get; private set; }
    
    private readonly List<BookAuthorEntity> _bookAuthors = [];
    public IReadOnlyCollection<BookAuthorEntity> BookAuthors => _bookAuthors.AsReadOnly();
    
    private readonly List<BookCategoryEntity> _bookCategories = [];
    public IReadOnlyCollection<BookCategoryEntity> BookCategories => _bookCategories.AsReadOnly();

    private BookEntity() { }

    private BookEntity(ISBNObject isbn, string title)
    {
        Id = Guid.CreateVersion7();
        ISBN = isbn;
        Title = title;
    }

    public void ChangeTitle(string title)
    {
        Title = title;
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
    }

    public void ChangePublicationYear(int publicationYear)
    {
        if (publicationYear < 0) throw new DomainException("Invalid publication year");
        PublicationYear = publicationYear;
    }

    public void ChangeCoverImage(ImageObject coverImage)
    {
        CoverImage = coverImage;
    }

    public void ChangeQuantity(int quantity)
    {
        if (quantity < 0) throw new DomainException("Invalid quantity");
        Quantity = quantity;
    }
    
    public void ChangePrice(MoneyObject price)
    {
        Price = price;
    }

    public void AssignPublisher(Guid publisherId)
    {
        PublisherId = publisherId;
    }

    public void RemovePublisher()
    {
        PublisherId = null;
    }

    public void AssignAuthor(Guid authorId)
    {
        if (_bookAuthors.All(x => x.AuthorId != authorId))
        {
            _bookAuthors.Add(BookAuthorEntity.Create(Id, authorId));
        }
    }

    public void RemoveAuthor(Guid authorId)
    {
        _bookAuthors.RemoveAll(x => x.AuthorId == authorId);
    }

    public void AssignCategory(Guid categoryId)
    {
        if (_bookCategories.All(x => x.CategoryId != categoryId))
        {
            _bookCategories.Add(BookCategoryEntity.Create(categoryId, Id));
        }
    }

    public void RemoveCategory(Guid categoryId)
    {
        _bookCategories.RemoveAll(x => x.CategoryId == categoryId);
    }
    
    public static BookEntity CreateBook(ISBNObject isbn, string title)
    {
        return new BookEntity(isbn, title);
    }
}