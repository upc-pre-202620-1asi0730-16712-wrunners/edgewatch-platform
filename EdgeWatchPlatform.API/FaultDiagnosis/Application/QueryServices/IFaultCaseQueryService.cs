using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Application.QueryServices;

public interface IFaultCaseQueryService
{
    Task<FaultCase?> Handle(GetFaultCaseByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<FaultCase>> Handle(GetAllFaultCasesByOrganizationIdQuery query, CancellationToken cancellationToken = default);
}
