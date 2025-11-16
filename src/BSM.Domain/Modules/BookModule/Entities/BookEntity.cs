using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.BookModule.ValueObjects;

namespace BSM.Domain.Modules.BookModule.Entities;

public class BookEntity : EntityBase, IAggregateRoot
{
    public string ISBN { get; private set; } = null!;
    public string Title { get; private set; } = null!;
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

    public static BookEntity Create(string isbn, string title)
    {
        ValidateIsbn(isbn);
        ValidateTitle(title);
        
        var book = new BookEntity
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow,
            ISBN = isbn,
            Title = title
        };
        
        return book;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    public void UpdatePublicationYear(int? year)
    {
        if (year.HasValue && year < 0)
            throw new DomainException("Publication year cannot be negative");
        
        PublicationYear = year;
    }

    public void UpdateCoverImage(ImageObject? coverImage)
    {
        CoverImage = coverImage;
    }

    public void UpdateQuantity(int? quantity)
    {
        if (quantity.HasValue && quantity < 0)
            throw new DomainException("Quantity cannot be negative");
        
        Quantity = quantity;
    }

    public void UpdatePrice(MoneyObject? price)
    {
        Price = price;
    }

    public void UpdatePublisher(Guid? publisherId)
    {
        PublisherId = publisherId;
    }

    public void AddAuthor(BookAuthorEntity author)
    {
        if (author == null)
            throw new DomainException("Author cannot be null");
        
        if (_bookAuthors.Any(ba => ba.AuthorId == author.AuthorId))
            throw new DomainException("Author already added to this book");
        
        _bookAuthors.Add(author);
    }

    public void RemoveAuthor(Guid authorId)
    {
        var author = _bookAuthors.FirstOrDefault(ba => ba.AuthorId == authorId);
        if (author != null)
            _bookAuthors.Remove(author);
    }

    public void AddCategory(BookCategoryEntity category)
    {
        if (category == null)
            throw new DomainException("Category cannot be null");
        
        if (_bookCategories.Any(bc => bc.CategoryId == category.CategoryId))
            throw new DomainException("Category already added to this book");
        
        _bookCategories.Add(category);
    }

    public void RemoveCategory(Guid categoryId)
    {
        var category = _bookCategories.FirstOrDefault(bc => bc.CategoryId == categoryId);
        if (category != null)
            _bookCategories.Remove(category);
    }

    public bool IsAvailable() => Quantity.HasValue && Quantity > 0;

    public bool HasEnoughQuantity(int requestedQuantity)
    {
        if (requestedQuantity <= 0)
            throw new DomainException("Requested quantity must be greater than zero");
        
        return Quantity.HasValue && Quantity >= requestedQuantity;
    }

    public void ReduceQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to reduce must be greater than zero");
        
        if (!HasEnoughQuantity(quantity))
            throw new DomainException($"Not enough quantity. Available: {Quantity}, Requested: {quantity}");
        
        Quantity -= quantity;
    }

    public void IncreaseQuantity(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to increase must be greater than zero");
        
        Quantity = (Quantity ?? 0) + quantity;
    }

    private static void ValidateIsbn(string isbn)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new DomainException("ISBN cannot be empty");
        
        if (isbn.Length < 10)
            throw new DomainException("ISBN must be at least 10 characters long");
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");
        
        if (title.Length > 500)
            throw new DomainException("Title cannot exceed 500 characters");
    }
}