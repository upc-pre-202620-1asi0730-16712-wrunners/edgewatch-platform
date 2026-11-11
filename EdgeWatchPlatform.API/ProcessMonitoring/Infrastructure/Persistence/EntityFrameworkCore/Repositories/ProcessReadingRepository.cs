using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class ProcessReadingRepository(AppDbContext context) : IProcessReadingRepository
{
    public async Task AddRangeAsync(IEnumerable<ProcessReading> readings, CancellationToken cancellationToken = default) =>
        await context.Set<ProcessReading>().AddRangeAsync(readings, cancellationToken);

    public async Task<IEnumerable<ProcessReading>> FindBySpraySessionIdAsync(int spraySessionId, Band? band, CancellationToken cancellationToken = default)
    {
        var query = context.Set<ProcessReading>().AsNoTracking().Where(r => r.SpraySessionId == spraySessionId);
        if (band is not null) query = query.Where(r => r.Band == band);
        return await query.OrderBy(r => r.EpochMillis).ThenBy(r => r.Id).ToListAsync(cancellationToken);
    }

    public async Task<int> CountBySpraySessionIdAndBandAsync(int spraySessionId, Band band, CancellationToken cancellationToken = default) =>
        await context.Set<ProcessReading>().CountAsync(r => r.SpraySessionId == spraySessionId && r.Band == band, cancellationToken);
}
