using BSM.Domain.Modules.ContentModule.Entities;
using BSM.Domain.Modules.ContentModule.Repositories;
using BSM.Infrastructure.Persistent;

namespace BSM.Infrastructure.ArRepositories;

public class CommentArRepository : ArRepository<CommentEntity>, ICommentArRepository
{
    public CommentArRepository(AppDbContext context) : base(context)
    {
    }
}
