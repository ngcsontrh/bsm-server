namespace BSM.Domain.Commons.Contracts;

public interface IAuditable
{
    DateTime CreatedAt { get; }
    Guid? CreatorId { get; }
    string? CreatorName { get; }
    DateTime? LastModifiedAt { get; }
    Guid? LastModifierId { get; }
    string? LastModifierName { get; }
}