using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

/// <summary>Updates the editable data of an HVOF system. The code and the organization are its identity and never change.</summary>
public record UpdateHvofSystemCommand(int HvofSystemId, string SerialNumber, string SystemManufacturer, string SystemModel, FuelType FuelType, HvofSystemStatus Status);
