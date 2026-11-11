using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;

/// <summary>A physical, replaceable part of a subsystem.</summary>
public class HvofPart
{
    protected HvofPart()
    {
        SerialNumber = string.Empty;
        Manufacturer = string.Empty;
    }

    public HvofPart(PartType partType, string serialNumber, string manufacturer) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(serialNumber);
        ArgumentException.ThrowIfNullOrWhiteSpace(manufacturer);
        PartType = partType;
        SerialNumber = serialNumber.Trim();
        Manufacturer = manufacturer.Trim();
    }

    public int Id { get; private set; }
    public int HvofSubsystemId { get; private set; }
    public PartType PartType { get; private set; }
    public string SerialNumber { get; private set; }
    public string Manufacturer { get; private set; }
}
