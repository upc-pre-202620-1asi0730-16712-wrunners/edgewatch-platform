using EdgeWatchPlatform.API.Shared.Domain.Repositories;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Repositories;

public interface ICustomerRepository : IBaseRepository<Customer>
{
    Task<IEnumerable<Customer>> FindBySupplierOrganizationIdAsync(int supplierOrganizationId, CancellationToken cancellationToken = default);
    Task<bool> ExistsBySupplierOrganizationIdAndRucAsync(int supplierOrganizationId, Ruc ruc, CancellationToken cancellationToken = default);
}
