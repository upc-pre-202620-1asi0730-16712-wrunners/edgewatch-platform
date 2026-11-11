using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Commands;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Entities;
using EdgeWatchPlatform.API.Shared.Application.Model;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Application.CommandServices;

public interface ISpraySessionCommandService
{
    Task<Result<SpraySession>> Handle(StartSpraySessionCommand command, CancellationToken cancellationToken = default);
    Task<Result<SpraySession>> Handle(CompleteSpraySessionCommand command, CancellationToken cancellationToken = default);
    Task<Result<SpraySession>> Handle(AbortSpraySessionCommand command, CancellationToken cancellationToken = default);
    Task<Result<IReadOnlyCollection<ProcessReading>>> Handle(IngestProcessReadingsCommand command, CancellationToken cancellationToken = default);
}
