using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

public record AddPartToSubsystemCommand(int HvofSystemId, int SubsystemId, PartType PartType, string SerialNumber, string Manufacturer);
