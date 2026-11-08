using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;

public record RegisterComponentCommand(int CustomerId, string SerialNumber, string PartNumber, ComponentType ComponentType,
    string MachineManufacturer, string MachineModel, int PcrTargetHours);
