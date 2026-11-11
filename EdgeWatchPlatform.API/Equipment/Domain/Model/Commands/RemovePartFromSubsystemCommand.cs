namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

public record RemovePartFromSubsystemCommand(int HvofSystemId, int SubsystemId, int PartId);
