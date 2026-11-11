using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;

/// <summary>A functional subsystem of an HVOF system with the parameters it measures and its parts.</summary>
public class HvofSubsystem
{
    private readonly List<SubsystemParameter> _parameters = [];
    private readonly List<HvofPart> _parts = [];

    protected HvofSubsystem()
    {
        Name = string.Empty;
        Alias = string.Empty;
    }

    public HvofSubsystem(SubsystemType subsystemType, string name, string alias, IEnumerable<ParameterName> parameters) : this()
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(name);
        ArgumentException.ThrowIfNullOrWhiteSpace(alias);
        SubsystemType = subsystemType;
        Name = name.Trim();
        Alias = alias.Trim();
        foreach (var parameter in parameters.DistinctBy(p => p.Value)) _parameters.Add(new SubsystemParameter(parameter.Value));
    }

    public int Id { get; private set; }
    public int HvofSystemId { get; private set; }
    public SubsystemType SubsystemType { get; private set; }
    public string Name { get; private set; }
    public string Alias { get; private set; }
    public IReadOnlyCollection<SubsystemParameter> Parameters => _parameters.AsReadOnly();
    public IReadOnlyCollection<HvofPart> Parts => _parts.AsReadOnly();

    public bool MeasuresParameter(string parameter) => _parameters.Any(p => p.Parameter == parameter);

    public bool HasPartWithSerialNumber(string serialNumber) =>
        _parts.Any(p => string.Equals(p.SerialNumber, serialNumber.Trim(), StringComparison.OrdinalIgnoreCase));

    /// <summary>Adds a part. Returns false when a part with the same serial number already exists.</summary>
    public bool TryAddPart(HvofPart part)
    {
        if (HasPartWithSerialNumber(part.SerialNumber)) return false;
        _parts.Add(part);
        return true;
    }
}
