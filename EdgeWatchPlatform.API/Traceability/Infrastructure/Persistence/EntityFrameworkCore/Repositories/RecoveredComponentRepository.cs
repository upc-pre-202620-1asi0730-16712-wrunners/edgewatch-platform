using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Traceability.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RecoveredComponentRepository(AppDbContext context) : BaseRepository<RecoveredComponent>(context), IRecoveredComponentRepository
{
    public async Task<IEnumerable<RecoveredComponent>> FindByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default) =>
        await Context.Set<RecoveredComponent>().Where(c => c.CustomerId == customerId).OrderBy(c => c.SerialNumber).ToListAsync(cancellationToken);

    public async Task<bool> ExistsByCustomerIdAndSerialNumberAsync(int customerId, string serialNumber, CancellationToken cancellationToken = default) =>
        await Context.Set<RecoveredComponent>().AnyAsync(c => c.CustomerId == customerId && c.SerialNumber == serialNumber, cancellationToken);
}
