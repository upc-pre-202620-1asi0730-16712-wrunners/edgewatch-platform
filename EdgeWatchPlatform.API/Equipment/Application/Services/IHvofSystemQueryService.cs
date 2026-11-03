using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Equipment.Application.Services;

public interface IHvofSystemQueryService
{
    Task<HvofSystem?> Handle(GetHvofSystemByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<HvofSystem>> Handle(GetAllHvofSystemsByOrganizationIdQuery query, CancellationToken cancellationToken = default);
}
