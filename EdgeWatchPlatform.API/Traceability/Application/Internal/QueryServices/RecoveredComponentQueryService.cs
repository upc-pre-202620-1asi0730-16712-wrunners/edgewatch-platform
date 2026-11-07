using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;

namespace EdgeWatchPlatform.API.Traceability.Application.Internal.QueryServices;

public class RecoveredComponentQueryService(IRecoveredComponentRepository componentRepository) : IRecoveredComponentQueryService
{
    public async Task<RecoveredComponent?> Handle(GetComponentByIdQuery query, CancellationToken cancellationToken = default) =>
        await componentRepository.FindByIdAsync(query.Id, cancellationToken);

    public async Task<IEnumerable<RecoveredComponent>> Handle(GetAllComponentsQuery query, CancellationToken cancellationToken = default) =>
        await componentRepository.ListAsync(cancellationToken);

    public async Task<IEnumerable<RecoveredComponent>> Handle(GetAllComponentsByCustomerIdQuery query, CancellationToken cancellationToken = default) =>
        await componentRepository.FindByCustomerIdAsync(query.CustomerId, cancellationToken);
}
