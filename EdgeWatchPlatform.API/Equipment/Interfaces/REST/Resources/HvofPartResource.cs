namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record HvofPartResource(int Id, int HvofSubsystemId, string PartType, string SerialNumber, string Manufacturer);
