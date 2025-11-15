using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;
using BSM.Domain.Commons.Exceptions;
using BSM.Domain.Modules.ContentModule.ValueObjects;

namespace BSM.Domain.Modules.ContentModule.Entities;

public class BlogEntity : EntityBase, IAggregateRoot
{
    public Guid AccountId { get; private set; }
    public string Title { get; private set; }
    public string Content { get; private set; }
    public BlogStatusObject Status { get; private set; }
    
    private BlogEntity() { }

    private BlogEntity(Guid accountId, string title, string content, BlogStatusObject status)
    {
        AccountId = accountId;
        Title = title;
        Content = content;
        Status = status;
    }

    public static BlogEntity Create(Guid accountId, string title, string content, BlogStatusObject status)
    {
        return new BlogEntity(accountId, title, content, status);
    }
    
    public void ChangeTitle(string title)
    {
        if (string.IsNullOrWhiteSpace(title) || title.Length > 100) throw new DomainException("Title is invalid");
        Title = title;
    }

    public void ChangeContent(string content)
    {
        Content = content;
    }

    public void ChangeStatus(BlogStatusObject status)
    {
        Status = status;
    }
}