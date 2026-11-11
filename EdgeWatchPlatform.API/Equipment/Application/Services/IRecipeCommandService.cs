using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Shared.Application.Model;

namespace EdgeWatchPlatform.API.Equipment.Application.Services;

public interface IRecipeCommandService
{
    Task<Result<Recipe>> Handle(CreateRecipeCommand command, CancellationToken cancellationToken = default);
    Task<Result<Recipe>> Handle(PublishRecipeCommand command, CancellationToken cancellationToken = default);
    Task<Result<Recipe>> Handle(RetireRecipeCommand command, CancellationToken cancellationToken = default);
}
