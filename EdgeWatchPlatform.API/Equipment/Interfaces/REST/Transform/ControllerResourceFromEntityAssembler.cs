using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class ControllerResourceFromEntityAssembler
{
    public static ControllerResource ToResourceFromEntity(Controller controller) =>
        new(controller.Id, controller.HvofSystemId, controller.ControllerNumber, controller.DeviceType.ToUpperSnakeCase(), controller.SerialNumber,
            controller.Manufacturer, controller.Model, controller.PartNumber, controller.IpAddress, controller.Port, controller.Rack, controller.Slot,
            controller.EndpointUrl, controller.SupportedProtocols.Select(p => new ProtocolResource(p.Protocol.ToUpperSnakeCase())).ToList());
}
