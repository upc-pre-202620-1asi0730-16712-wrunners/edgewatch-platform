using EdgeWatchPlatform.API.Shared.Domain.Model;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;

public partial class RecoveredComponent : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
