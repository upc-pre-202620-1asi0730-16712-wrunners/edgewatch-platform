using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Equipment.Domain.Repositories;

public interface IRecipeRepository : IBaseRepository<Recipe>
{
    Task<Recipe?> FindByIdWithParametersAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recipe>> FindByHvofSystemIdAsync(int hvofSystemId, CancellationToken cancellationToken = default);
    Task<Recipe?> FindActiveByHvofSystemIdAndNumberAsync(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default);
    Task<bool> ExistsDraftByHvofSystemIdAndNumberAsync(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default);
}
