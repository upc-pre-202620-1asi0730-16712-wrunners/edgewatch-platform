namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

/// <summary>Activates a DRAFT recipe; the previous ACTIVE recipe with the same number is retired.</summary>
public record PublishRecipeCommand(int RecipeId);
