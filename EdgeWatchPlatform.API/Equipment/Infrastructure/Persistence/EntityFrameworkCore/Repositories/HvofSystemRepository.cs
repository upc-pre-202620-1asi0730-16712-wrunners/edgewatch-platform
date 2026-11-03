using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Equipment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class HvofSystemRepository(AppDbContext context) : BaseRepository<HvofSystem>(context), IHvofSystemRepository
{
    public async Task<IEnumerable<HvofSystem>> FindByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default) =>
        await Context.Set<HvofSystem>().Where(s => s.OrganizationId == organizationId).OrderBy(s => s.Code).ToListAsync(cancellationToken);

    public async Task<bool> ExistsByOrganizationIdAndCodeAsync(int organizationId, HvofSystemCode code, CancellationToken cancellationToken = default) =>
        await Context.Set<HvofSystem>().AnyAsync(s => s.OrganizationId == organizationId && s.Code == code, cancellationToken);
}
