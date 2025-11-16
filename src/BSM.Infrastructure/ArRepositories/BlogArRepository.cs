using BSM.Domain.Modules.ContentModule.Entities;
using BSM.Domain.Modules.ContentModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class BlogArRepository : ArRepository<BlogEntity>, IBlogArRepository
{
    public BlogArRepository(AppDbContext context) : base(context)
    {
    }
}
