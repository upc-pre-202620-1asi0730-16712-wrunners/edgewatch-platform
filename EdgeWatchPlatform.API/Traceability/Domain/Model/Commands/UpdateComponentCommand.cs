using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

public record UpdateComponentCommand(int ComponentId, string PartNumber, ComponentType ComponentType, string MachineManufacturer,
    string MachineModel, int PcrTargetHours, ComponentStatus Status);
