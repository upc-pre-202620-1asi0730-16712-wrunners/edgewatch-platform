using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class RegisterHvofSystemCommandFromResourceAssembler
{
    public static RegisterHvofSystemCommand ToCommandFromResource(RegisterHvofSystemResource resource) =>
        new(resource.OrganizationId, new HvofSystemCode(resource.Code), resource.SerialNumber, resource.SystemManufacturer,
            resource.SystemModel, Enum.Parse<FuelType>(resource.FuelType.Replace("_", ""), true));
}
