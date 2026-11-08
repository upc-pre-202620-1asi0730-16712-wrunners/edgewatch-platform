using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;

/// <summary>
///     A customer's mining component that goes through HVOF recuperation.
///     Named RecoveredComponent to avoid clashing with System.ComponentModel.Component; stored in the <c>components</c> table.
/// </summary>
public partial class RecoveredComponent
{
    protected RecoveredComponent()
    {
        SerialNumber = string.Empty;
        PartNumber = string.Empty;
        MachineManufacturer = string.Empty;
        MachineModel = string.Empty;
    }

    public RecoveredComponent(RegisterComponentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        CustomerId = command.CustomerId;
        SerialNumber = command.SerialNumber.Trim();
        PartNumber = command.PartNumber.Trim();
        ComponentType = command.ComponentType;
        MachineManufacturer = command.MachineManufacturer.Trim();
        MachineModel = command.MachineModel.Trim();
        PcrTargetHours = command.PcrTargetHours;
        Status = ComponentStatus.Received;
    }

    public int Id { get; private set; }
    public int CustomerId { get; private set; }
    public string SerialNumber { get; private set; }
    public string PartNumber { get; private set; }
    public ComponentType ComponentType { get; private set; }
    public string MachineManufacturer { get; private set; }
    public string MachineModel { get; private set; }
    public int PcrTargetHours { get; private set; }
    public ComponentStatus Status { get; private set; }

    /// <summary>Updates the editable data. The serial number and the customer are the component identity and never change.</summary>
    public void Update(UpdateComponentCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        PartNumber = command.PartNumber.Trim();
        ComponentType = command.ComponentType;
        MachineManufacturer = command.MachineManufacturer.Trim();
        MachineModel = command.MachineModel.Trim();
        PcrTargetHours = command.PcrTargetHours;
        Status = command.Status;
    }

    public void MarkInRecuperation() => Status = ComponentStatus.InRecuperation;
    public void MarkInService() => Status = ComponentStatus.InService;
}
