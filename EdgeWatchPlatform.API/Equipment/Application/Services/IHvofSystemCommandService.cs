using EdgeWatchPlatform.API.Equipment.Application.Errors;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Shared.Application.Patterns;

namespace EdgeWatchPlatform.API.Equipment.Application.Services;

public interface IHvofSystemCommandService
{
    Task<Result<HvofSystem, RegisterHvofSystemError>> Handle(RegisterHvofSystemCommand command, CancellationToken cancellationToken = default);
}
