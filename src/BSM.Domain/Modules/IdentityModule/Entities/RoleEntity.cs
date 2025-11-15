using BSM.Domain.Commons.Contracts;
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

    public void ChangeDescription(string description)
    {
        Description = description;
    }
}
