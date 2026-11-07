using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

namespace EdgeWatchPlatform.API.Traceability.Application.Services;

public interface IRecuperationCommandService
{
    Task<Result<Recuperation, OpenRecuperationError>> Handle(OpenRecuperationCommand command, CancellationToken cancellationToken = default);
    Task<Result<Recuperation, UpdateRecuperationError>> Handle(UpdateRecuperationCommand command, CancellationToken cancellationToken = default);
    Task<Result<Recuperation, ChangeRecuperationStatusError>> Handle(ChangeRecuperationStatusCommand command, CancellationToken cancellationToken = default);
    Task<Result<Recuperation, LinkSessionToRecuperationError>> Handle(LinkSessionToRecuperationCommand command, CancellationToken cancellationToken = default);
}
