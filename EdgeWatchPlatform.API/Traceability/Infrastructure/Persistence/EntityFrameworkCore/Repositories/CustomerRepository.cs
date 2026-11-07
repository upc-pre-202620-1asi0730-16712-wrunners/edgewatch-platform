using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Traceability.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class CustomerRepository(AppDbContext context) : BaseRepository<Customer>(context), ICustomerRepository
{
    public async Task<IEnumerable<Customer>> FindBySupplierOrganizationIdAsync(int supplierOrganizationId, CancellationToken cancellationToken = default) =>
        await Context.Set<Customer>().Where(c => c.SupplierOrganizationId == supplierOrganizationId).OrderBy(c => c.LegalName).ToListAsync(cancellationToken);

    public async Task<bool> ExistsBySupplierOrganizationIdAndRucAsync(int supplierOrganizationId, Ruc ruc, CancellationToken cancellationToken = default) =>
        await Context.Set<Customer>().AnyAsync(c => c.SupplierOrganizationId == supplierOrganizationId && c.Ruc == ruc, cancellationToken);
}
