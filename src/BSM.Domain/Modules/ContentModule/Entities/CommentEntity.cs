using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.ContentModule.Entities;

public class CommentEntity : EntityBase, IAggregateRoot
{
    public Guid BlogId { get; private set; }
    public Guid AccountId { get; private set; }
    public string Content { get; private set; }
    public Guid? ParentId { get; private set; }
    public CommentEntity Parent { get; private set; }
    
    
    private CommentEntity() { }

    private CommentEntity(Guid blogId, Guid accountId, string content)
    {
        BlogId = blogId;
        AccountId = accountId;
        Content = content;
    }

    public static CommentEntity Create(Guid blogId, Guid accountId, string content)
    {
        return new CommentEntity(blogId, accountId, content);
    }

    public void ChangeContent(string newContent)
    {
        Content = newContent;
    }

    public void AssignParent(Guid parentId)
    {
        if (ParentId != null) throw new DomainException("Parent already assigned");
        ParentId = parentId;
    }
}