using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Domain.Repositories;

/// <summary>Readings are append-only and queried by session; they are not loaded through the aggregate.</summary>
public interface IProcessReadingRepository
{
    Task AddRangeAsync(IEnumerable<ProcessReading> readings, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProcessReading>> FindBySpraySessionIdAsync(int spraySessionId, Band? band, CancellationToken cancellationToken = default);
    Task<int> CountBySpraySessionIdAndBandAsync(int spraySessionId, Band band, CancellationToken cancellationToken = default);
}
