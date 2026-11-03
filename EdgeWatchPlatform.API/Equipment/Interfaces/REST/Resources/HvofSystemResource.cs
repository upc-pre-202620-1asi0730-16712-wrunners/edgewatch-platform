namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

/// <summary>Representation of an HVOF system.</summary>
public record HvofSystemResource(int Id, string Code, int OrganizationId, string SerialNumber, string Status,
    string SystemManufacturer, string SystemModel, string FuelType);
