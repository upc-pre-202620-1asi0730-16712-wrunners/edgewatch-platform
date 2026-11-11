using EdgeWatchPlatform.API.Traceability.Interfaces.Acl;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.OutboundServices.Acl;

/// <summary>Outbound service that talks to Traceability only through its context facade.</summary>
public class ExternalTraceabilityService(ITraceabilityContextFacade traceabilityContextFacade)
{
    public async Task<int?> FetchRecuperationSupplierOrganizationId(int recuperationId, CancellationToken cancellationToken = default)
    {
        var organizationId = await traceabilityContextFacade.FetchRecuperationSupplierOrganizationId(recuperationId, cancellationToken);
        return organizationId == 0 ? null : organizationId;
    }

    public Task<bool> LinkSessionToRecuperation(int recuperationId, int sessionId, CancellationToken cancellationToken = default) =>
        traceabilityContextFacade.LinkSessionToRecuperation(recuperationId, sessionId, cancellationToken);
}
