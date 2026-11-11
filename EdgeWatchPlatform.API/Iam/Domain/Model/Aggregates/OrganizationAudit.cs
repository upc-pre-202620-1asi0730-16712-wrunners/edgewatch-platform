using EdgeWatchPlatform.API.Shared.Domain.Model;

namespace EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;

public partial class Organization : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
