using EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Iam.Domain.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<User?> FindByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);
    Task<bool> ExistsByEmailAsync(EmailAddress email, CancellationToken cancellationToken = default);
    Task<IEnumerable<User>> FindByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default);
}
