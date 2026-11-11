using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Iam.Domain.Repositories;

public interface IOrganizationRepository : IBaseRepository<Organization>
{
    Task<bool> ExistsByRucAsync(Ruc ruc, CancellationToken cancellationToken = default);
}
