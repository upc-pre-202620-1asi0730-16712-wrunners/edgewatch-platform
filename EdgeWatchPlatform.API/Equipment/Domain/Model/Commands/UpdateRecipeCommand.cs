using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

/// <summary>Replaces the data of a DRAFT recipe (name, powder, applicabilities and parameters).</summary>
public record UpdateRecipeCommand(int RecipeId, string Name, string PowderSpecification, IReadOnlyCollection<RecipeApplicability> Applicabilities,
    IReadOnlyCollection<RecipeParameterItem> Parameters);
