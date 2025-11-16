using BSM.Domain.Modules.IdentityModule.Entities;
using BSM.Domain.Modules.IdentityModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class AccountArRepository : ArRepository<AccountEntity>, IAccountArRepository
{
    public AccountArRepository(AppDbContext context) : base(context)
    {
    }
}
