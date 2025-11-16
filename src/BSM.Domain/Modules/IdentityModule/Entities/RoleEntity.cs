using BSM.Domain.Commons.Contracts;
using BSM.Domain.Commons.Exceptions;
using Microsoft.AspNetCore.Identity;

namespace BSM.Domain.Modules.IdentityModule.Entities;

public class RoleEntity : IdentityRole<Guid>, IAuditable
{
    public DateTime CreatedAt { get; private set; }
    public Guid? CreatorId { get; private set; }
    public string? CreatorName { get; private set; }
    public DateTime? LastModifiedAt { get; private set; }
    public Guid? LastModifierId { get; private set; }
    public string? LastModifierName { get; private set; }
    public string? Description { get; private set; }

    private RoleEntity() { }

    public static RoleEntity Create(string name)
    {
        ValidateName(name);

        var role = new RoleEntity
        {
            Id = Guid.CreateVersion7(),
            Name = name,
            NormalizedName = name.ToUpperInvariant(),
            ConcurrencyStamp = Guid.NewGuid().ToString()
        };

        return role;
    }

    public void UpdateDescription(string? description)
    {
        Description = description;
    }

    private static void ValidateName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Role name cannot be empty");

        if (name.Length > 256)
            throw new DomainException("Role name cannot exceed 256 characters");
    }
}
