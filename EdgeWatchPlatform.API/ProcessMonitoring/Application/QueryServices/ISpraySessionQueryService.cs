using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Application.QueryServices;

public interface ISpraySessionQueryService
{
    Task<SpraySession?> Handle(GetSpraySessionByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<SpraySession>> Handle(GetAllSpraySessionsByOrganizationIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<SpraySession>> Handle(GetAllSpraySessionsByRecuperationIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<ProcessReading>> Handle(GetAllReadingsBySpraySessionIdQuery query, CancellationToken cancellationToken = default);

    /// <summary>Warning and shutdown counts of a session.</summary>
    Task<(int warning, int shutdown)> Handle(GetDeviationCountBySpraySessionIdQuery query, CancellationToken cancellationToken = default);
}
