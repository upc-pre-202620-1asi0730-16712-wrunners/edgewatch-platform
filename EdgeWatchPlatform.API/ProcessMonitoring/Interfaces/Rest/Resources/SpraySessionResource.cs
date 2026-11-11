namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;

public record SpraySessionResource(int Id, int OrganizationId, int HvofSystemId, int RecuperationId, int OperatorId, int RecipeId, int RecipeNumber,
    DateTimeOffset StartedAt, DateTimeOffset? EndedAt, string TimeZone, string Status, string? AbortReason);
