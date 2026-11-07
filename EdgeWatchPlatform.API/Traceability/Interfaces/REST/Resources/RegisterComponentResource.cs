using System.ComponentModel.DataAnnotations;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Validation;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Request body to register a component of a customer.</summary>
public record RegisterComponentResource(
    [Required(ErrorMessage = "RequiredCustomerId")] [Range(1, int.MaxValue, ErrorMessage = "InvalidCustomerId")] int CustomerId,
    [Required(ErrorMessage = "RequiredSerialNumber")] [MaxLength(60)] string SerialNumber,
    [Required(ErrorMessage = "RequiredPartNumber")] [MaxLength(40)] string PartNumber,
    [Required(ErrorMessage = "RequiredComponentType")] [EnumValue<ComponentType>(ErrorMessage = "InvalidComponentType")] string ComponentType,
    [Required(ErrorMessage = "RequiredMachineManufacturer")] [MaxLength(80)] string MachineManufacturer,
    [Required(ErrorMessage = "RequiredMachineModel")] [MaxLength(80)] string MachineModel,
    [Range(1, 100000, ErrorMessage = "InvalidPcrTargetHours")] int PcrTargetHours);
