namespace EdgeWatchPlatform.API.Shared.Domain.Model;

/// <summary>Marks an entity whose CreatedAt/UpdatedAt are managed by the persistence layer.</summary>
public interface IAuditableEntity
{
    DateTimeOffset? CreatedAt { get; set; }
    DateTimeOffset? UpdatedAt { get; set; }
}