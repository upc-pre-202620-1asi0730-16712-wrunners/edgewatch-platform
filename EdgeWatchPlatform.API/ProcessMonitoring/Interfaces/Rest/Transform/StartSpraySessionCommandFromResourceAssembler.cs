using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Commands;
using EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Transform;

public static class StartSpraySessionCommandFromResourceAssembler
{
    public static StartSpraySessionCommand ToCommandFromResource(StartSpraySessionResource resource, int operatorId) =>
        new(resource.HvofSystemId, resource.RecuperationId, operatorId, resource.RecipeNumber, resource.TimeZone ?? "America/Lima");
}
