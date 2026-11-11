using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;

namespace EdgeWatchPlatform.API.Traceability.Domain.Repositories;

public interface IRecoveredComponentRepository : IBaseRepository<RecoveredComponent>
{
    Task<IEnumerable<RecoveredComponent>> FindByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<bool> ExistsByCustomerIdAndSerialNumberAsync(int customerId, string serialNumber, CancellationToken cancellationToken = default);
}
