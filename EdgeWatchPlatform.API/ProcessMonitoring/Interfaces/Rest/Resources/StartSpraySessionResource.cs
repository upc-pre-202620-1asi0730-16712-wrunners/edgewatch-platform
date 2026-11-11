using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;

public record StartSpraySessionResource(
    [Range(1, int.MaxValue, ErrorMessage = "InvalidHvofSystemId")] int HvofSystemId,
    [Range(1, int.MaxValue, ErrorMessage = "InvalidRecuperationId")] int RecuperationId,
    [Range(1, 9999, ErrorMessage = "InvalidRecipeNumber")] int RecipeNumber,
    [MaxLength(60)] string? TimeZone);
