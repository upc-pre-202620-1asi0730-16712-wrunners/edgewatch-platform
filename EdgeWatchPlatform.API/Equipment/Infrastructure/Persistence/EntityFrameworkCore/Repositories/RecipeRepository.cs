using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Repositories;
using Microsoft.EntityFrameworkCore;

namespace EdgeWatchPlatform.API.Equipment.Infrastructure.Persistence.EntityFrameworkCore.Repositories;

public class RecipeRepository(AppDbContext context) : BaseRepository<Recipe>(context), IRecipeRepository
{
    public async Task<Recipe?> FindByIdWithParametersAsync(int id, CancellationToken cancellationToken = default) =>
        await Context.Set<Recipe>().Include(r => r.Parameters).FirstOrDefaultAsync(r => r.Id == id, cancellationToken);

    public async Task<IEnumerable<Recipe>> FindByHvofSystemIdAsync(int hvofSystemId, CancellationToken cancellationToken = default) =>
        await Context.Set<Recipe>().Include(r => r.Parameters).Where(r => r.HvofSystemId == hvofSystemId)
            .OrderBy(r => r.RecipeNumber).ThenByDescending(r => r.Id).ToListAsync(cancellationToken);

    public async Task<Recipe?> FindActiveByHvofSystemIdAndNumberAsync(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default) =>
        await Context.Set<Recipe>().Include(r => r.Parameters)
            .FirstOrDefaultAsync(r => r.HvofSystemId == hvofSystemId && r.RecipeNumber == recipeNumber && r.Status == RecipeStatus.Active, cancellationToken);

    public async Task<bool> ExistsDraftByHvofSystemIdAndNumberAsync(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default) =>
        await Context.Set<Recipe>().AnyAsync(r => r.HvofSystemId == hvofSystemId && r.RecipeNumber == recipeNumber && r.Status == RecipeStatus.Draft, cancellationToken);
}
