using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class CreateRecipeCommandFromResourceAssembler
{
    public static CreateRecipeCommand ToCommandFromResource(CreateRecipeResource resource) =>
        new(resource.HvofSystemId, resource.RecipeNumber, resource.Name, resource.PowderSpecification,
            (resource.Applicabilities ?? []).Select(a => new RecipeApplicability(a.ComponentType.Trim().ToUpperInvariant(), a.MachineModel.Trim(), a.Position.Trim())).ToList(),
            resource.Parameters.Select(p => new RecipeParameterItem(new ParameterName(p.Parameter),
                new ThresholdBands(p.Setpoint, p.LowerShutdown, p.LowerWarning, p.NominalMin, p.NominalMax, p.UpperWarning, p.UpperShutdown),
                p.UnitSymbol, p.UnitCategory.ToEnum<UnitCategory>())).ToList());
}
