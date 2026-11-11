using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Transform;

public static class SpraySessionResourceFromEntityAssembler
{
    public static SpraySessionResource ToResourceFromEntity(SpraySession session) =>
        new(session.Id, session.OrganizationId, session.HvofSystemId, session.RecuperationId, session.OperatorId, session.RecipeId, session.RecipeNumber,
            session.StartedAt, session.EndedAt, session.TimeZone, session.Status.ToUpperSnakeCase().ToLowerInvariant(),
            session.AbortReason?.ToUpperSnakeCase());
}
