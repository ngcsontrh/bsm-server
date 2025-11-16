using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;

namespace BSM.Domain.Modules.ContentModule.Entities;

public class CommentEntity : EntityBase, IAggregateRoot
{
    public Guid BlogId { get; private set; }
    public Guid AccountId { get; private set; }
    public string Content { get; private set; } = null!;
    public Guid? ParentId { get; private set; }

    private CommentEntity() { }

    public static CommentEntity Create(Guid blogId, Guid accountId, string content, Guid? parentId = null)
    {
        if (blogId == Guid.Empty)
            throw new DomainException("BlogId cannot be empty");

        if (accountId == Guid.Empty)
            throw new DomainException("AccountId cannot be empty");

        ValidateContent(content);

        var comment = new CommentEntity
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow,
            BlogId = blogId,
            AccountId = accountId,
            Content = content,
            ParentId = parentId
        };

        return comment;
    }

    public void UpdateContent(string content)
    {
        ValidateContent(content);
        Content = content;
    }

    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Content cannot be empty");

        if (content.Length > 2000)
            throw new DomainException("Content cannot exceed 2000 characters");
    }
}