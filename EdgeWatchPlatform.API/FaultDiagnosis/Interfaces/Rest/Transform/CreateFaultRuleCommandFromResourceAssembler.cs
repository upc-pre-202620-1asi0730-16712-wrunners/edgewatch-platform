using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Transform;

public static class CreateFaultRuleCommandFromResourceAssembler
{
    public static CreateFaultRuleCommand ToCommandFromResource(int organizationId, CreateFaultRuleResource resource) =>
        new(organizationId, resource.Parameter, resource.Band.ToEnum<FaultBand>(), resource.ProbableCause, resource.RecommendedAction, resource.CooldownSeconds);
}
