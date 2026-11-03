using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;

public record RegisterHvofSystemCommand(int OrganizationId, HvofSystemCode Code, string SerialNumber,
    string SystemManufacturer, string SystemModel, FuelType FuelType);
