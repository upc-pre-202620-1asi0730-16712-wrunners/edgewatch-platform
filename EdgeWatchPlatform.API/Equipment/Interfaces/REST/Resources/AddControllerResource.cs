using System.ComponentModel.DataAnnotations;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record AddControllerResource(
    [Range(1, 99, ErrorMessage = "InvalidControllerNumber")] int ControllerNumber,
    [Required(ErrorMessage = "RequiredDeviceType")] [RegularExpression("^(PLC|OPC_UA_SERVER|GATEWAY)$", ErrorMessage = "InvalidDeviceType")] string DeviceType,
    [Required(ErrorMessage = "RequiredSerialNumber")] [MaxLength(60)] string SerialNumber,
    [Required(ErrorMessage = "RequiredManufacturer")] [MaxLength(80)] string Manufacturer,
    [Required(ErrorMessage = "RequiredModel")] [MaxLength(80)] string Model,
    [MaxLength(40)] string? PartNumber,
    [Required(ErrorMessage = "RequiredIpAddress")] [MaxLength(45)] string IpAddress,
    [Range(1, 65535, ErrorMessage = "InvalidPort")] int Port,
    [Range(0, 99)] int Rack,
    [Range(0, 99)] int Slot,
    [MaxLength(200)] string? EndpointUrl,
    IReadOnlyCollection<ProtocolResource>? SupportedProtocols);
