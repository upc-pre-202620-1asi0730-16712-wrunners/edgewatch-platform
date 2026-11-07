using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Repositories;

public interface IRecuperationRepository : IBaseRepository<Recuperation>
{
    Task<IEnumerable<Recuperation>> FindBySupplierOrganizationIdAsync(int supplierOrganizationId, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recuperation>> FindByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySupplierOrganizationIdAndWorkOrderNumberAsync(int supplierOrganizationId, WorkOrderNumber workOrderNumber, CancellationToken cancellationToken = default);
}
