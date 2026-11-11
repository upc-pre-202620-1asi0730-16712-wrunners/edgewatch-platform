using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;
using EdgeWatchPlatform.API.Iam.Domain.Repositories;

namespace EdgeWatchPlatform.API.Iam.Application.Internal.QueryServices;

public class RoleQueryService(IRoleRepository roleRepository) : IRoleQueryService
{
    public async Task<IEnumerable<Role>> Handle(GetAllRolesQuery query, CancellationToken cancellationToken = default) =>
        await roleRepository.ListAsync(cancellationToken);
}
