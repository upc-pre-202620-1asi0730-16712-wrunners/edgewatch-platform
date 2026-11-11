using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Equipment.Domain.Repositories;

public interface IHvofSystemRepository : IBaseRepository<HvofSystem>
{
    Task<IEnumerable<HvofSystem>> FindByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByOrganizationIdAndCodeAsync(int organizationId, HvofSystemCode code, CancellationToken cancellationToken = default);

    /// <summary>Loads the system with its subsystems (and parts) and controllers.</summary>
    Task<HvofSystem?> FindByIdWithCompositionAsync(int id, CancellationToken cancellationToken = default);
}
