using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Transform;

public static class FaultCaseResourceFromEntityAssembler
{
    public static FaultCaseResource ToResourceFromEntity(FaultCase c) =>
        new(c.Id, c.OrganizationId, c.SpraySessionId, c.HvofSystemId, c.Parameter, c.Band.ToUpperSnakeCase().ToLowerInvariant(), c.Value, c.EpochMillis,
            c.ProbableCause, c.RecommendedAction, c.Status.ToUpperSnakeCase().ToLowerInvariant(), c.AcknowledgedBy, c.AcknowledgedAt, c.ResolvedBy, c.ResolvedAt,
            c.ResolutionNotes);
}
