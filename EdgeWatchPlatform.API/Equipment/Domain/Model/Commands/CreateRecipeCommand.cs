using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

public record RecipeParameterItem(ParameterName Parameter, ThresholdBands Bands, string UnitSymbol, UnitCategory UnitCategory);

public record CreateRecipeCommand(int HvofSystemId, int RecipeNumber, string Name, string PowderSpecification,
    IReadOnlyCollection<RecipeApplicability> Applicabilities, IReadOnlyCollection<RecipeParameterItem> Parameters);
