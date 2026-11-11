namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

public record HvofSubsystemResource(int Id, int HvofSystemId, string SubsystemType, string Name, string Alias,
    IEnumerable<ParameterNameResource> Parameters, IEnumerable<HvofPartResource> Parts);
