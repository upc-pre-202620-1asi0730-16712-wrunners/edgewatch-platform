using EdgeWatchPlatform.API.Equipment.Interfaces.Acl;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.OutboundServices.Acl;

/// <summary>Outbound service that talks to Equipment only through its context facade.</summary>
public class ExternalEquipmentService(IEquipmentContextFacade equipmentContextFacade)
{
    public async Task<int?> FetchHvofSystemOrganizationId(int hvofSystemId, CancellationToken cancellationToken = default)
    {
        var organizationId = await equipmentContextFacade.FetchHvofSystemOrganizationId(hvofSystemId, cancellationToken);
        return organizationId == 0 ? null : organizationId;
    }

    public async Task<int?> FetchActiveRecipeId(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default)
    {
        var recipeId = await equipmentContextFacade.FetchActiveRecipeId(hvofSystemId, recipeNumber, cancellationToken);
        return recipeId == 0 ? null : recipeId;
    }

    public async Task<IReadOnlyCollection<SessionRecipeBand>> FetchActiveRecipeBands(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default)
    {
        var bands = await equipmentContextFacade.FetchActiveRecipeBands(hvofSystemId, recipeNumber, cancellationToken);
        return bands.Select(b => new SessionRecipeBand(b.Parameter, b.Setpoint, b.LowerShutdown, b.LowerWarning, b.NominalMin, b.NominalMax,
            b.UpperWarning, b.UpperShutdown, b.UnitSymbol, b.UnitCategory)).ToList();
    }
}
