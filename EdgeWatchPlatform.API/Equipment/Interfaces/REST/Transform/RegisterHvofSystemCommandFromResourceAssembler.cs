using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class RegisterHvofSystemCommandFromResourceAssembler
{
    public static RegisterHvofSystemCommand ToCommandFromResource(RegisterHvofSystemResource resource) =>
        new(resource.OrganizationId, new HvofSystemCode(resource.Code), resource.SerialNumber, resource.SystemManufacturer,
            resource.SystemModel, resource.FuelType.ToEnum<FuelType>());
}
