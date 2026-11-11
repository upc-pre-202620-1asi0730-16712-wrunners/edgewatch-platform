using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class FaultCaseRepository(AppDbContext context) : BaseRepository<FaultCase>(context), IFaultCaseRepository
{
    public async Task<IEnumerable<FaultCase>> FindByOrganizationIdAsync(int organizationId, int? spraySessionId, FaultCaseStatus? status, CancellationToken cancellationToken = default)
    {
        var query = Context.Set<FaultCase>().Where(c => c.OrganizationId == organizationId);
        if (spraySessionId is not null) query = query.Where(c => c.SpraySessionId == spraySessionId);
        if (status is not null) query = query.Where(c => c.Status == status);
        return await query.OrderByDescending(c => c.EpochMillis).ToListAsync(cancellationToken);
    }

    public async Task<FaultCase?> FindLatestBySpraySessionIdAndParameterAndBandAsync(int spraySessionId, string parameter, FaultBand band, CancellationToken cancellationToken = default)
    {
        // Cases added in the current unit of work (not yet saved) count for the cooldown too.
        var pending = Context.ChangeTracker.Entries<FaultCase>()
            .Where(e => e.State == EntityState.Added && e.Entity.SpraySessionId == spraySessionId && e.Entity.Parameter == parameter && e.Entity.Band == band)
            .Select(e => e.Entity).OrderByDescending(c => c.EpochMillis).FirstOrDefault();
        if (pending is not null) return pending;
        return await Context.Set<FaultCase>().Where(c => c.SpraySessionId == spraySessionId && c.Parameter == parameter && c.Band == band)
            .OrderByDescending(c => c.EpochMillis).FirstOrDefaultAsync(cancellationToken);
    }
}
