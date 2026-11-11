using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;

/// <summary>A PLC, OPC UA server or gateway attached to an HVOF system.</summary>
public class Controller
{
    private readonly List<SupportedProtocol> _supportedProtocols = [];

    protected Controller()
    {
        SerialNumber = string.Empty;
        Manufacturer = string.Empty;
        Model = string.Empty;
        PartNumber = string.Empty;
        IpAddress = string.Empty;
    }

    public Controller(int controllerNumber, DeviceType deviceType, string serialNumber, string manufacturer, string model, string partNumber,
        string ipAddress, int port, int rack, int slot, string? endpointUrl, IEnumerable<Protocol> protocols) : this()
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(controllerNumber, 1);
        ArgumentException.ThrowIfNullOrWhiteSpace(serialNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(port);
        ControllerNumber = controllerNumber;
        DeviceType = deviceType;
        SerialNumber = serialNumber.Trim();
        Manufacturer = manufacturer.Trim();
        Model = model.Trim();
        PartNumber = partNumber.Trim();
        IpAddress = ipAddress.Trim();
        Port = port;
        Rack = rack;
        Slot = slot;
        EndpointUrl = string.IsNullOrWhiteSpace(endpointUrl) ? null : endpointUrl.Trim();
        foreach (var protocol in protocols.Distinct()) _supportedProtocols.Add(new SupportedProtocol(protocol));
    }

    public Controller Update(DeviceType deviceType, string serialNumber, string manufacturer, string model, string partNumber, string ipAddress, int port,
        int rack, int slot, string? endpointUrl, IEnumerable<Protocol> protocols)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serialNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(ipAddress);
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(port);
        DeviceType = deviceType;
        SerialNumber = serialNumber.Trim();
        Manufacturer = manufacturer.Trim();
        Model = model.Trim();
        PartNumber = partNumber.Trim();
        IpAddress = ipAddress.Trim();
        Port = port;
        Rack = rack;
        Slot = slot;
        EndpointUrl = string.IsNullOrWhiteSpace(endpointUrl) ? null : endpointUrl.Trim();
        _supportedProtocols.Clear();
        foreach (var protocol in protocols.Distinct()) _supportedProtocols.Add(new SupportedProtocol(protocol));
        return this;
    }

    public int Id { get; private set; }
    public int HvofSystemId { get; private set; }
    public int ControllerNumber { get; private set; }
    public DeviceType DeviceType { get; private set; }
    public string SerialNumber { get; private set; }
    public string Manufacturer { get; private set; }
    public string Model { get; private set; }
    public string PartNumber { get; private set; }
    public string IpAddress { get; private set; }
    public int Port { get; private set; }
    public int Rack { get; private set; }
    public int Slot { get; private set; }
    public string? EndpointUrl { get; private set; }
    public IReadOnlyCollection<SupportedProtocol> SupportedProtocols => _supportedProtocols.AsReadOnly();
}
