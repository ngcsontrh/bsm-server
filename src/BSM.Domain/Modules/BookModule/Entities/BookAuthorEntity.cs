using BSM.Domain.Commons.Contracts;

namespace BSM.Domain.Modules.BookModule.Entities;

public class BookAuthorEntity : IEntity
{
    public Guid BookId { get; private set; }
    public BookEntity? Book { get; private set; }
    public Guid AuthorId { get; private set; }
    public AuthorEntity? Author { get; private set; }
}