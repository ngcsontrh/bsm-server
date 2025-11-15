using BSM.Domain.Commons.Contracts;

namespace BSM.Domain.Modules.BookModule.Entities;

public class BookCategoryEntity : IEntity
{
    public Guid BookId { get; private set; }
    public BookEntity Book { get; private set; }
    
    public Guid CategoryId { get; private set; }
    public CategoryEntity Category { get; private set; }

    private BookCategoryEntity(Guid bookId, Guid categoryId)
    {
        BookId = bookId;
        CategoryId = categoryId;
    }
    
    private BookCategoryEntity()
    {
    }

    public static BookCategoryEntity Create(Guid bookId, Guid categoryId)
    {
        return new BookCategoryEntity(bookId, categoryId);
    }
}