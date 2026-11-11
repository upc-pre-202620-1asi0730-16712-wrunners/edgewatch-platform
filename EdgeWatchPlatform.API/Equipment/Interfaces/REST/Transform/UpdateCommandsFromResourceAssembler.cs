using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;

/// <summary>Builds the update commands of the Equipment context from their resources.</summary>
public static class UpdateCommandsFromResourceAssembler
{
    public static UpdateHvofSystemCommand ToCommandFromResource(int hvofSystemId, UpdateHvofSystemResource resource) =>
        new(hvofSystemId, resource.SerialNumber, resource.SystemManufacturer, resource.SystemModel, resource.FuelType.ToEnum<FuelType>(),
            resource.Status.ToEnum<HvofSystemStatus>());

    public static UpdateSubsystemCommand ToCommandFromResource(int hvofSystemId, int subsystemId, AddSubsystemResource resource) =>
        new(hvofSystemId, subsystemId, resource.SubsystemType.ToEnum<SubsystemType>(), resource.Name, resource.Alias,
            (resource.Parameters ?? []).Select(p => new ParameterName(p.Parameter)).ToList());

    public static UpdateControllerCommand ToCommandFromResource(int hvofSystemId, int controllerId, AddControllerResource resource) =>
        new(hvofSystemId, controllerId, resource.DeviceType.ToEnum<DeviceType>(), resource.SerialNumber, resource.Manufacturer, resource.Model,
            resource.PartNumber ?? string.Empty, resource.IpAddress, resource.Port, resource.Rack, resource.Slot, resource.EndpointUrl,
            (resource.SupportedProtocols ?? []).Select(p => p.Protocol.ToEnum<Protocol>()).ToList());

    public static UpdateRecipeCommand ToCommandFromResource(int recipeId, UpdateRecipeResource resource) =>
        new(recipeId, resource.Name, resource.PowderSpecification,
            (resource.Applicabilities ?? []).Select(a => new RecipeApplicability(a.ComponentType.Trim().ToUpperInvariant(), a.MachineModel.Trim(), a.Position.Trim())).ToList(),
            resource.Parameters.Select(p => new RecipeParameterItem(new ParameterName(p.Parameter),
                new ThresholdBands(p.Setpoint, p.LowerShutdown, p.LowerWarning, p.NominalMin, p.NominalMax, p.UpperWarning, p.UpperShutdown),
                p.UnitSymbol, p.UnitCategory.ToEnum<UnitCategory>())).ToList());
}
