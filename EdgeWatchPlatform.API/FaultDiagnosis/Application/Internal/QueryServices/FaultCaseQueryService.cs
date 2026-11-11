using EdgeWatchPlatform.API.FaultDiagnosis.Application.QueryServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Queries;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.Internal.QueryServices;

public class FaultCaseQueryService(IFaultCaseRepository caseRepository) : IFaultCaseQueryService
{
    public async Task<FaultCase?> Handle(GetFaultCaseByIdQuery query, CancellationToken cancellationToken = default) =>
        await caseRepository.FindByIdAsync(query.FaultCaseId, cancellationToken);

    public async Task<IEnumerable<FaultCase>> Handle(GetAllFaultCasesByOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await caseRepository.FindByOrganizationIdAsync(query.OrganizationId, query.SpraySessionId, query.Status, cancellationToken);
}
