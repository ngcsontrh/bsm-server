using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Entities;

namespace BSM.Domain.Modules.BookModule.Entities;

public class CategoryEntity : EntityBase, IAggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    
    private CategoryEntity() { }
    
    private CategoryEntity(string name, string? description)
    {
        Name = name;
        Description = description;
    }

    public static CategoryEntity Create(string name, string? description)
    {
        return new CategoryEntity(name, description);
    }

    public void ChangeName(string name)
    {
        Name = name;
    }

    public void ChangeDescription(string? description)
    {
        Description = description;
    }
}