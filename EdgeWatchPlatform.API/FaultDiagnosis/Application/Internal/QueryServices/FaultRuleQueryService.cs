using EdgeWatchPlatform.API.FaultDiagnosis.Application.QueryServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Queries;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.Internal.QueryServices;

public class FaultRuleQueryService(IFaultRuleRepository ruleRepository) : IFaultRuleQueryService
{
    public async Task<IEnumerable<FaultRule>> Handle(GetAllFaultRulesByOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await ruleRepository.FindByOrganizationIdAsync(query.OrganizationId, cancellationToken);
}
