namespace BSM.Domain.Commons.Contracts;

public interface ISoftDelete<out T>
{
    DateTimeOffset? DeletedAt { get; }
    T? DeletedById { get; }
    string? DeletedByName { get; }
}