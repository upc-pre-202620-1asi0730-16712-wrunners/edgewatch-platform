using EdgeWatchPlatform.API.Shared.Domain.Model;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;

public partial class SpraySession : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
