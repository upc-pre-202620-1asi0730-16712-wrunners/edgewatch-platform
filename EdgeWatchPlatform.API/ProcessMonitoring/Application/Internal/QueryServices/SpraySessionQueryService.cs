using EdgeWatchPlatform.API.ProcessMonitoring.Application.QueryServices;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Queries;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Repositories;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Application.Internal.QueryServices;

public class SpraySessionQueryService(ISpraySessionRepository sessionRepository, IProcessReadingRepository readingRepository) : ISpraySessionQueryService
{
    public async Task<SpraySession?> Handle(GetSpraySessionByIdQuery query, CancellationToken cancellationToken = default) =>
        await sessionRepository.FindByIdAsync(query.SpraySessionId, cancellationToken);

    public async Task<IEnumerable<SpraySession>> Handle(GetAllSpraySessionsByOrganizationIdQuery query, CancellationToken cancellationToken = default) =>
        await sessionRepository.FindByOrganizationIdAsync(query.OrganizationId, cancellationToken);

    public async Task<IEnumerable<SpraySession>> Handle(GetAllSpraySessionsByRecuperationIdQuery query, CancellationToken cancellationToken = default) =>
        await sessionRepository.FindByRecuperationIdAsync(query.RecuperationId, cancellationToken);

    public async Task<IEnumerable<ProcessReading>> Handle(GetAllReadingsBySpraySessionIdQuery query, CancellationToken cancellationToken = default) =>
        await readingRepository.FindBySpraySessionIdAsync(query.SpraySessionId, query.Band, cancellationToken);

    public async Task<(int warning, int shutdown)> Handle(GetDeviationCountBySpraySessionIdQuery query, CancellationToken cancellationToken = default)
    {
        var warning = await readingRepository.CountBySpraySessionIdAndBandAsync(query.SpraySessionId, Band.Warning, cancellationToken);
        var shutdown = await readingRepository.CountBySpraySessionIdAndBandAsync(query.SpraySessionId, Band.Shutdown, cancellationToken);
        return (warning, shutdown);
    }
}
