using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;
using EdgeWatchPlatform.API.Equipment.Domain.Repositories;

namespace EdgeWatchPlatform.API.Equipment.Application.Internal.QueryServices;

public class RecipeQueryService(IRecipeRepository recipeRepository) : IRecipeQueryService
{
    public async Task<Recipe?> Handle(GetRecipeByIdQuery query, CancellationToken cancellationToken = default) =>
        await recipeRepository.FindByIdWithParametersAsync(query.RecipeId, cancellationToken);

    public async Task<IEnumerable<Recipe>> Handle(GetAllRecipesByHvofSystemIdQuery query, CancellationToken cancellationToken = default) =>
        await recipeRepository.FindByHvofSystemIdAsync(query.HvofSystemId, cancellationToken);

    public async Task<Recipe?> Handle(GetActiveRecipeByHvofSystemIdAndNumberQuery query, CancellationToken cancellationToken = default) =>
        await recipeRepository.FindActiveByHvofSystemIdAndNumberAsync(query.HvofSystemId, query.RecipeNumber, cancellationToken);
}
