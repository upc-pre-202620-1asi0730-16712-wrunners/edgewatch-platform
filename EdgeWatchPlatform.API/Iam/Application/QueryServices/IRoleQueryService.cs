using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Iam.Application.QueryServices;

public interface IRoleQueryService
{
    Task<IEnumerable<Role>> Handle(GetAllRolesQuery query, CancellationToken cancellationToken = default);
}
