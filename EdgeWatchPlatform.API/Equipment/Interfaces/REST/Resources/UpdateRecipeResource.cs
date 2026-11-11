using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

/// <summary>Request body to update a DRAFT recipe.</summary>
public record UpdateRecipeResource(
    [Required(ErrorMessage = "RequiredName")] [MaxLength(120)] string Name,
    [Required(ErrorMessage = "RequiredPowderSpecification")] [MaxLength(200)] string PowderSpecification,
    IReadOnlyCollection<RecipeApplicabilityResource>? Applicabilities,
    [Required] [MinLength(1, ErrorMessage = "RequiredParameters")] IReadOnlyCollection<RecipeParameterResource> Parameters);
