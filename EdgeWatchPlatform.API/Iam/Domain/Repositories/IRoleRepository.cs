using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Iam.Domain.Repositories;

public interface IRoleRepository : IBaseRepository<Role>
{
    Task<Role?> FindByNameAsync(Roles name, CancellationToken cancellationToken = default);
    Task<bool> ExistsByNameAsync(Roles name, CancellationToken cancellationToken = default);
    Task<IEnumerable<Role>> FindByIdsAsync(IEnumerable<int> ids, CancellationToken cancellationToken = default);
}
