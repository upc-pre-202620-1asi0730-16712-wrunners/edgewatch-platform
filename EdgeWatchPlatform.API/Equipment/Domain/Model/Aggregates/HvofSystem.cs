using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Events;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;

/// <summary>An HVOF thermal spray system owned by a recuperation supplier organization.</summary>
public partial class HvofSystem : AggregateRoot
{
    protected HvofSystem()
    {
        Code = null!;
        SerialNumber = string.Empty;
        SystemManufacturer = string.Empty;
        SystemModel = string.Empty;
    }

    public HvofSystem(RegisterHvofSystemCommand command) : this()
    {
        ArgumentNullException.ThrowIfNull(command);
        OrganizationId = command.OrganizationId;
        Code = command.Code;
        SerialNumber = command.SerialNumber.Trim();
        SystemManufacturer = command.SystemManufacturer.Trim();
        SystemModel = command.SystemModel.Trim();
        FuelType = command.FuelType;
        Status = HvofSystemStatus.Active;
        AddDomainEvent(new HvofSystemRegisteredEvent(OrganizationId, Code.Value, SerialNumber));
    }

    public int Id { get; private set; }
    public int OrganizationId { get; private set; }
    public HvofSystemCode Code { get; private set; }
    public string SerialNumber { get; private set; }
    public string SystemManufacturer { get; private set; }
    public string SystemModel { get; private set; }
    public FuelType FuelType { get; private set; }
    public HvofSystemStatus Status { get; private set; }

    public void SetStatus(HvofSystemStatus status) => Status = status;
}
