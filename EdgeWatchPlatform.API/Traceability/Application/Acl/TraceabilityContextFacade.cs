using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;
using EdgeWatchPlatform.API.Traceability.Interfaces.Acl;

namespace EdgeWatchPlatform.API.Traceability.Application.Acl;

public class TraceabilityContextFacade(IRecuperationQueryService recuperationQueryService, IRecuperationCommandService recuperationCommandService)
    : ITraceabilityContextFacade
{
    public async Task<int> FetchRecuperationSupplierOrganizationId(int recuperationId, CancellationToken cancellationToken = default)
    {
        var recuperation = await recuperationQueryService.Handle(new GetRecuperationByIdQuery(recuperationId), cancellationToken);
        return recuperation?.SupplierOrganizationId ?? 0;
    }

    public async Task<bool> LinkSessionToRecuperation(int recuperationId, int sessionId, CancellationToken cancellationToken = default)
    {
        var result = await recuperationCommandService.Handle(new LinkSessionToRecuperationCommand(recuperationId, sessionId), cancellationToken);
        return result.IsSuccess;
    }
}
