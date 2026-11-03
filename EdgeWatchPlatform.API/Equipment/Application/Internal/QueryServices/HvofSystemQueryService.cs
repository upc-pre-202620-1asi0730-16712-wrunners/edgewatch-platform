using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;
using EdgeWatchPlatform.API.Equipment.Domain.Repositories;

namespace EdgeWatchPlatform.API.Equipment.Application.Internal.QueryServices;

public class HvofSystemQueryService(IHvofSystemRepository hvofSystemRepository) : IHvofSystemQueryService
{
    public async Task<HvofSystem?> Handle(GetHvofSystemByIdQuery query, CancellationToken cancellationToken = default) =>
        await hvofSystemRepository.FindByIdAsync(query.Id, cancellationToken);

    public async Task<IEnumerable<HvofSystem>> Handle(GetAllHvofSystemsByOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await hvofSystemRepository.FindByOrganizationIdAsync(query.OrganizationId, cancellationToken);
}
