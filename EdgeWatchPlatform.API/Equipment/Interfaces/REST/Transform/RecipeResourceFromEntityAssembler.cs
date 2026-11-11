using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class RecipeResourceFromEntityAssembler
{
    public static RecipeResource ToResourceFromEntity(Recipe recipe) =>
        new(recipe.Id, recipe.HvofSystemId, recipe.RecipeNumber, recipe.Name, recipe.PowderSpecification, recipe.Status.ToUpperSnakeCase(),
            recipe.Applicabilities.Select(a => new RecipeApplicabilityResource(a.ComponentType, a.MachineModel, a.Position)).ToList(),
            recipe.Parameters.OrderBy(p => p.Id).Select(p => new RecipeParameterResource(p.Parameter, p.Setpoint, p.LowerShutdown, p.LowerWarning,
                p.NominalMin, p.NominalMax, p.UpperWarning, p.UpperShutdown, p.UnitSymbol, p.UnitCategory.ToUpperSnakeCase().ToLowerInvariant())).ToList());
}
