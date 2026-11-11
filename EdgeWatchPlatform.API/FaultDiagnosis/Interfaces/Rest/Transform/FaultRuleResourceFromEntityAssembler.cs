using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Transform;

public static class FaultRuleResourceFromEntityAssembler
{
    public static FaultRuleResource ToResourceFromEntity(FaultRule rule) =>
        new(rule.Id, rule.OrganizationId, rule.Parameter, rule.Band.ToUpperSnakeCase().ToLowerInvariant(), rule.ProbableCause, rule.RecommendedAction, rule.CooldownSeconds);
}
