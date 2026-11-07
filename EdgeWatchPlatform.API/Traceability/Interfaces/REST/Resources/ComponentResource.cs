namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

/// <summary>Representation of a recovered component.</summary>
public record ComponentResource(int Id, string SerialNumber, string PartNumber, string ComponentType, string MachineManufacturer,
    string MachineModel, int CustomerId, int PcrTargetHours, string Status);
