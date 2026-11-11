using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record AddSubsystemResource(
    [Required(ErrorMessage = "RequiredSubsystemType")] [RegularExpression("^(GAS_CONSOLE|POWDER_FEEDER|COOLING_UNIT|MANIPULATOR|DUST_COLLECTOR|SPRAY_GUN)$", ErrorMessage = "InvalidSubsystemType")] string SubsystemType,
    [Required(ErrorMessage = "RequiredName")] [MaxLength(80)] string Name,
    [Required(ErrorMessage = "RequiredAlias")] [MaxLength(30)] string Alias,
    IReadOnlyCollection<ParameterNameResource>? Parameters);
