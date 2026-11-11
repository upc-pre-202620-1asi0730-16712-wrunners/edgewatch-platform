using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Repositories;

public interface ISpraySessionRepository : IBaseRepository<SpraySession>
{
    Task<IEnumerable<SpraySession>> FindByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<SpraySession>> FindByRecuperationIdAsync(int recuperationId, CancellationToken cancellationToken = default);
}
