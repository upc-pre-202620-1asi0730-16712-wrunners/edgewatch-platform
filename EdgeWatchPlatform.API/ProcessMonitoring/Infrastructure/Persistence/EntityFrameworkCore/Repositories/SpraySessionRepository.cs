using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class SpraySessionRepository(AppDbContext context) : BaseRepository<SpraySession>(context), ISpraySessionRepository
{
    public async Task<IEnumerable<SpraySession>> FindByOrganizationIdAsync(int organizationId, CancellationToken cancellationToken = default) =>
        await Context.Set<SpraySession>().Where(s => s.OrganizationId == organizationId).OrderByDescending(s => s.StartedAt).ToListAsync(cancellationToken);

    public async Task<IEnumerable<SpraySession>> FindByRecuperationIdAsync(int recuperationId, CancellationToken cancellationToken = default) =>
        await Context.Set<SpraySession>().Where(s => s.RecuperationId == recuperationId).OrderByDescending(s => s.StartedAt).ToListAsync(cancellationToken);
}
