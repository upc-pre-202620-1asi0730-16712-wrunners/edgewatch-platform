using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record AddPartResource(
    [Required(ErrorMessage = "RequiredPartType")] [RegularExpression("^(MASS_FLOW_CONTROLLER|HOPPER|CARRIER_GAS_MFC|COOLANT_PUMP|TEMPERATURE_SENSOR|SPINDLE_VFD|AXIS_DRIVE|NOZZLE)$", ErrorMessage = "InvalidPartType")] string PartType,
    [Required(ErrorMessage = "RequiredSerialNumber")] [MaxLength(60)] string SerialNumber,
    [Required(ErrorMessage = "RequiredManufacturer")] [MaxLength(80)] string Manufacturer);
