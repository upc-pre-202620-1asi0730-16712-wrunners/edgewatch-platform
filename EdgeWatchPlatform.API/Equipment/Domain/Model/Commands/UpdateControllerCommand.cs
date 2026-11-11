using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

public record UpdateControllerCommand(int HvofSystemId, int ControllerId, DeviceType DeviceType, string SerialNumber, string Manufacturer, string Model,
    string PartNumber, string IpAddress, int Port, int Rack, int Slot, string? EndpointUrl, IReadOnlyCollection<Protocol> Protocols);
