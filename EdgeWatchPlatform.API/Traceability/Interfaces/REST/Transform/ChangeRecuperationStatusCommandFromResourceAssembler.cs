using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Commands;
using EdgeWatchPlatform.API.Traceability.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;

public static class ChangeRecuperationStatusCommandFromResourceAssembler
{
    public static ChangeRecuperationStatusCommand ToCommandFromResource(int id, ChangeRecuperationStatusResource resource) =>
        new(id, resource.Status.ToEnum<RecuperationStatus>());
}
