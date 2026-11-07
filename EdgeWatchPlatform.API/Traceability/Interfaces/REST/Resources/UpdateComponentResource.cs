using System.ComponentModel.DataAnnotations;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Validation;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Request body to update a component. Serial number and customer are not editable and are ignored if sent.</summary>
public record UpdateComponentResource(
    [Required(ErrorMessage = "RequiredPartNumber")] [MaxLength(40)] string PartNumber,
    [Required(ErrorMessage = "RequiredComponentType")] [EnumValue<ComponentType>(ErrorMessage = "InvalidComponentType")] string ComponentType,
    [Required(ErrorMessage = "RequiredMachineManufacturer")] [MaxLength(80)] string MachineManufacturer,
    [Required(ErrorMessage = "RequiredMachineModel")] [MaxLength(80)] string MachineModel,
    [Range(1, 100000, ErrorMessage = "InvalidPcrTargetHours")] int PcrTargetHours,
    [Required(ErrorMessage = "RequiredStatus")] [EnumValue<ComponentStatus>(ErrorMessage = "InvalidComponentStatus")] string Status);
