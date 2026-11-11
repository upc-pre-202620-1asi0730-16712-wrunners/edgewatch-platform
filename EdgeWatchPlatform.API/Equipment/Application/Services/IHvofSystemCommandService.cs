using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Shared.Application.Model;

namespace EdgeWatchPlatform.API.Equipment.Application.Services;

public interface IHvofSystemCommandService
{
    Task<Result<HvofSystem>> Handle(RegisterHvofSystemCommand command, CancellationToken cancellationToken = default);
}
