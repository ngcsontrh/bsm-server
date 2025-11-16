using BSM.Domain.Modules.ContentModule.Entities;
using BSM.Domain.Modules.ContentModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class EventArRepository : ArRepository<EventEntity>, IEventArRepository
{
    public EventArRepository(AppDbContext context) : base(context)
    {
    }
}
