using BSM.Domain.Modules.BookModule.Entities;
using BSM.Domain.Modules.BookModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class AuthorArRepository : ArRepository<AuthorEntity>, IAuthorArRepository
{
    public AuthorArRepository(AppDbContext context) : base(context)
    {
    }
}
