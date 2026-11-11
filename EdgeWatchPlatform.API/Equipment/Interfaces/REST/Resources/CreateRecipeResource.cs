using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record CreateRecipeResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidHvofSystemId")] int HvofSystemId,
    [Range(1, 9999, ErrorMessage = "InvalidRecipeNumber")] int RecipeNumber,
    [Required(ErrorMessage = "RequiredName")] [MaxLength(120)] string Name,
    [Required(ErrorMessage = "RequiredPowderSpecification")] [MaxLength(200)] string PowderSpecification,
    IReadOnlyCollection<RecipeApplicabilityResource>? Applicabilities,
    [Required] [MinLength(1, ErrorMessage = "RequiredParameters")] IReadOnlyCollection<RecipeParameterResource> Parameters);
