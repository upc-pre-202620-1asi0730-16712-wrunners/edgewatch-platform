namespace EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

/// <summary>Component type, machine model and position a recipe applies to (owned by the recipe).</summary>
public record RecipeApplicability(string ComponentType, string MachineModel, string Position);
