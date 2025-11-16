using BSM.Domain.Modules.BookModule.Entities;
using BSM.Domain.Modules.BookModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class BookArRepository : ArRepository<BookEntity>, IBookArRepository
{
    public BookArRepository(AppDbContext context) : base(context)
    {
    }
}
