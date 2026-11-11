using EdgeWatchPlatform.API.Shared.Domain.Model;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;

public partial class Recipe : IAuditableEntity
{
    public DateTimeOffset? CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}
