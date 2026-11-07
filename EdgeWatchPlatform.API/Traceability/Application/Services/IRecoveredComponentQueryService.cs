using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Traceability.Application.Services;

public interface IRecoveredComponentQueryService
{
    Task<RecoveredComponent?> Handle(GetComponentByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<RecoveredComponent>> Handle(GetAllComponentsQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<RecoveredComponent>> Handle(GetAllComponentsByCustomerIdQuery query, CancellationToken cancellationToken = default);
}
