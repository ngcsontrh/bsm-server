using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.ContentModule.ValueObjects;

namespace BSM.Domain.Modules.ContentModule.Entities;

public class BlogEntity : EntityBase, IAggregateRoot
{
    public Guid AccountId { get; private set; }
    public string Title { get; private set; } = null!;
    public string Content { get; private set; } = null!;
    public BlogStatusObject Status { get; private set; } = null!;

    private BlogEntity() { }

    public static BlogEntity Create(Guid accountId, string title, string content)
    {
        if (accountId == Guid.Empty)
            throw new DomainException("AccountId cannot be empty");

        ValidateTitle(title);
        ValidateContent(content);

        var blog = new BlogEntity
        {
            Id = Guid.CreateVersion7(),
            CreatedAt = DateTime.UtcNow,
            AccountId = accountId,
            Title = title,
            Content = content,
            Status = BlogStatusObject.Draft
        };

        return blog;
    }

    public void UpdateTitle(string title)
    {
        ValidateTitle(title);
        Title = title;
    }

    public void UpdateContent(string content)
    {
        ValidateContent(content);
        Content = content;
    }

    public void Publish()
    {
        Status = BlogStatusObject.Published;
    }

    public void Archive()
    {
        Status = BlogStatusObject.Archived;
    }

    public void SaveAsDraft()
    {
        Status = BlogStatusObject.Draft;
    }

    private static void ValidateTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new DomainException("Title cannot be empty");

        if (title.Length > 500)
            throw new DomainException("Title cannot exceed 500 characters");
    }

    private static void ValidateContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            throw new DomainException("Content cannot be empty");
    }
}