using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record RecipeApplicabilityResource(
    [Required(ErrorMessage = "RequiredComponentType")] [MaxLength(30)] string ComponentType,
    [Required(ErrorMessage = "RequiredMachineModel")] [MaxLength(80)] string MachineModel,
    [Required(ErrorMessage = "RequiredPosition")] [MaxLength(80)] string Position);
