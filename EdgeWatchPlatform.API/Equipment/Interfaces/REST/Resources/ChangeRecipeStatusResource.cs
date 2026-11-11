using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

/// <summary>Target status of a recipe: ACTIVE publishes a draft, RETIRED retires an active recipe.</summary>
public record ChangeRecipeStatusResource(
    [Required(ErrorMessage = "RequiredStatus")] [RegularExpression("^(ACTIVE|RETIRED)$", ErrorMessage = "InvalidRecipeStatus")] string Status);
