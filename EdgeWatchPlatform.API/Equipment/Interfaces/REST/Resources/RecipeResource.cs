namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record RecipeResource(int Id, int HvofSystemId, int RecipeNumber, string Name, string PowderSpecification, string Status,
    IEnumerable<RecipeApplicabilityResource> Applicabilities, IEnumerable<RecipeParameterResource> Parameters);
