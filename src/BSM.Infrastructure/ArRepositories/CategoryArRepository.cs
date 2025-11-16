using BSM.Domain.Modules.BookModule.Entities;
using BSM.Domain.Modules.BookModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class CategoryArRepository : ArRepository<CategoryEntity>, ICategoryArRepository
{
    public CategoryArRepository(AppDbContext context) : base(context)
    {
    }
}
