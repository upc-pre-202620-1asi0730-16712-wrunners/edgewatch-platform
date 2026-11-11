using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class AddControllerCommandFromResourceAssembler
{
    public static AddControllerToHvofSystemCommand ToCommandFromResource(int hvofSystemId, AddControllerResource resource) =>
        new(hvofSystemId, resource.ControllerNumber, resource.DeviceType.ToEnum<DeviceType>(), resource.SerialNumber, resource.Manufacturer,
            resource.Model, resource.PartNumber ?? string.Empty, resource.IpAddress, resource.Port, resource.Rack, resource.Slot, resource.EndpointUrl,
            (resource.SupportedProtocols ?? []).Select(p => p.Protocol.ToEnum<Protocol>()).ToList());
}
