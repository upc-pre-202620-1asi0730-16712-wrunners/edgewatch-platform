using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.QueryServices;

public interface IFaultRuleQueryService
{
    Task<IEnumerable<FaultRule>> Handle(GetAllFaultRulesByOrganizationIdQuery query, CancellationToken cancellationToken = default);
}
