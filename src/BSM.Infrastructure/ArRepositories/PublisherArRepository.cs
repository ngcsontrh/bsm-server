using BSM.Domain.Modules.BookModule.Entities;
using BSM.Domain.Modules.BookModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class PublisherArRepository : ArRepository<PublisherEntity>, IPublisherArRepository
{
    public PublisherArRepository(AppDbContext context) : base(context)
    {
    }
}
