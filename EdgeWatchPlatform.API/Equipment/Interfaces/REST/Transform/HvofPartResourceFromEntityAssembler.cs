using EdgeWatchPlatform.API.Equipment.Domain.Model.Entities;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

public static class HvofPartResourceFromEntityAssembler
{
    public static HvofPartResource ToResourceFromEntity(HvofPart part) =>
        new(part.Id, part.HvofSubsystemId, part.PartType.ToUpperSnakeCase(), part.SerialNumber, part.Manufacturer);
}
