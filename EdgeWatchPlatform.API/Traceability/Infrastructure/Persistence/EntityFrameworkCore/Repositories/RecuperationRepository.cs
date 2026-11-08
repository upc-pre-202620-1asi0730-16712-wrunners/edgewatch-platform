using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Traceability.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

/// <remarks>LinkedSessions is an owned collection, so EF Core loads it with every recuperation query (including FindByIdAsync).</remarks>
public class RecuperationRepository(AppDbContext context) : BaseRepository<Recuperation>(context), IRecuperationRepository
{
    public async Task<IEnumerable<Recuperation>> FindBySupplierOrganizationIdAsync(int supplierOrganizationId, CancellationToken cancellationToken = default) =>
        await Context.Set<Recuperation>().Where(r => r.SupplierOrganizationId == supplierOrganizationId).OrderByDescending(r => r.ReceivedAt).ToListAsync(cancellationToken);

    public async Task<IEnumerable<Recuperation>> FindByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default) =>
        await Context.Set<Recuperation>().Where(r => r.CustomerId == customerId).OrderByDescending(r => r.ReceivedAt).ToListAsync(cancellationToken);

    public async Task<bool> ExistsBySupplierOrganizationIdAndWorkOrderNumberAsync(int supplierOrganizationId, WorkOrderNumber workOrderNumber, CancellationToken cancellationToken = default) =>
        await Context.Set<Recuperation>().AnyAsync(r => r.SupplierOrganizationId == supplierOrganizationId && r.WorkOrderNumber == workOrderNumber, cancellationToken);
}
