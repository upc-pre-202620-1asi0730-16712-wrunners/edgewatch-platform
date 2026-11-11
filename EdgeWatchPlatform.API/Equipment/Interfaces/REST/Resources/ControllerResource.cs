namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record ControllerResource(int Id, int HvofSystemId, int ControllerNumber, string DeviceType, string SerialNumber, string Manufacturer,
    string Model, string PartNumber, string IpAddress, int Port, int Rack, int Slot, string? EndpointUrl, IEnumerable<ProtocolResource> SupportedProtocols);
