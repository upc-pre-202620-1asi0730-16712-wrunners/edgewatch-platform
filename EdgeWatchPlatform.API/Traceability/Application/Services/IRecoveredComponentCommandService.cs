using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

namespace EdgeWatchPlatform.API.Traceability.Application.Services;

public interface IRecoveredComponentCommandService
{
    Task<Result<RecoveredComponent, RegisterComponentError>> Handle(RegisterComponentCommand command, CancellationToken cancellationToken = default);
    Task<Result<RecoveredComponent, UpdateComponentError>> Handle(UpdateComponentCommand command, CancellationToken cancellationToken = default);
}
