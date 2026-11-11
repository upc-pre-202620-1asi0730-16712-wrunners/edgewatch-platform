using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;
using EdgeWatchPlatform.API.Equipment.Interfaces.Acl;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Application.Acl;

public class EquipmentContextFacade(IHvofSystemQueryService hvofSystemQueryService, IRecipeQueryService recipeQueryService) : IEquipmentContextFacade
{
    public async Task<int> FetchHvofSystemOrganizationId(int hvofSystemId, CancellationToken cancellationToken = default)
    {
        var system = await hvofSystemQueryService.Handle(new GetHvofSystemByIdQuery(hvofSystemId), cancellationToken);
        return system?.OrganizationId ?? 0;
    }

    public async Task<int> FetchActiveRecipeId(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default)
    {
        var recipe = await recipeQueryService.Handle(new GetActiveRecipeByHvofSystemIdAndNumberQuery(hvofSystemId, recipeNumber), cancellationToken);
        return recipe?.Id ?? 0;
    }

    public async Task<IReadOnlyCollection<RecipeBandSnapshot>> FetchActiveRecipeBands(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default)
    {
        var recipe = await recipeQueryService.Handle(new GetActiveRecipeByHvofSystemIdAndNumberQuery(hvofSystemId, recipeNumber), cancellationToken);
        if (recipe is null) return [];
        return recipe.Parameters.Select(p => new RecipeBandSnapshot(p.Parameter, p.Setpoint, p.LowerShutdown, p.LowerWarning, p.NominalMin,
            p.NominalMax, p.UpperWarning, p.UpperShutdown, p.UnitSymbol, p.UnitCategory.ToUpperSnakeCase().ToLowerInvariant())).ToList();
    }
}
