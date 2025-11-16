using BSM.Domain.Modules.IdentityModule.Entities;
using BSM.Domain.Modules.IdentityModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class RoleArRepository : ArRepository<RoleEntity>, IRoleArRepository
{
    public RoleArRepository(AppDbContext context) : base(context)
    {
    }
}
