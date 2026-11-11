using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;

/// <summary>Editable data of the HVOF system.</summary>
public partial class HvofSystem
{
    public HvofSystem Update(UpdateHvofSystemCommand command)
    {
        ArgumentNullException.ThrowIfNull(command);
        ArgumentException.ThrowIfNullOrWhiteSpace(command.SerialNumber);
        SerialNumber = command.SerialNumber.Trim();
        SystemManufacturer = command.SystemManufacturer.Trim();
        SystemModel = command.SystemModel.Trim();
        FuelType = command.FuelType;
        Status = command.Status;
        return this;
    }

    /// <summary>Removes a part of a subsystem. Returns false when the subsystem or the part does not exist.</summary>
    public bool TryRemovePart(int subsystemId, int partId)
    {
        var subsystem = FindSubsystemById(subsystemId);
        return subsystem is not null && subsystem.TryRemovePart(partId);
    }
}
