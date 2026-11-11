using System.Net.Mime;
using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST;

/// <summary>Subsystems and parts of an HVOF system.</summary>
[Authorize]
[ApiController]
[Route("api/v1/hvof-systems/{hvofSystemId:int}/subsystems")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("HVOF Systems")]
public class HvofSystemSubsystemsController(
    IHvofSystemCommandService commandService,
    IHvofSystemQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Lists the subsystems of an HVOF system with their parameters and parts.</summary>
    [HttpGet]
    [EndpointSummary("Get the subsystems of an HVOF system")]
    [ProducesResponseType(typeof(IEnumerable<HvofSubsystemResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllSubsystemsByHvofSystemId(int hvofSystemId, CancellationToken cancellationToken)
    {
        var subsystems = await queryService.Handle(new GetAllSubsystemsByHvofSystemIdQuery(hvofSystemId), cancellationToken);
        return EquipmentActionResultAssembler.ToActionResultFromSystemCollection(this, subsystems, problemDetailsFactory,
            items => Ok(items.Select(HvofSubsystemResourceFromEntityAssembler.ToResourceFromEntity)), hvofSystemId);
    }

    /// <summary>Adds a subsystem to an HVOF system.</summary>
    [HttpPost]
    [EndpointSummary("Add a subsystem to an HVOF system")]
    [ProducesResponseType(typeof(HvofSubsystemResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddSubsystem(int hvofSystemId, [FromBody] AddSubsystemResource resource, CancellationToken cancellationToken)
    {
        var command = AddSubsystemCommandFromResourceAssembler.ToCommandFromResource(hvofSystemId, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return EquipmentActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            subsystem => Created($"/api/v1/hvof-systems/{hvofSystemId}/subsystems", HvofSubsystemResourceFromEntityAssembler.ToResourceFromEntity(subsystem)),
            result.Error is Domain.Model.EquipmentError.HvofSystemNotFound ? (object)hvofSystemId : resource.Alias);
    }

    /// <summary>Updates a subsystem.</summary>
    [HttpPut("{subsystemId:int}")]
    [EndpointSummary("Update a subsystem")]
    [ProducesResponseType(typeof(HvofSubsystemResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdateSubsystem(int hvofSystemId, int subsystemId, [FromBody] AddSubsystemResource resource, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(UpdateCommandsFromResourceAssembler.ToCommandFromResource(hvofSystemId, subsystemId, resource), cancellationToken);
        return EquipmentActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            subsystem => Ok(HvofSubsystemResourceFromEntityAssembler.ToResourceFromEntity(subsystem)),
            result.Error is Domain.Model.EquipmentError.DuplicateSubsystemAlias ? resource.Alias : subsystemId);
    }

    /// <summary>Removes a part from a subsystem.</summary>
    [HttpDelete("{subsystemId:int}/parts/{partId:int}")]
    [EndpointSummary("Remove a part from a subsystem")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> RemovePart(int hvofSystemId, int subsystemId, int partId, CancellationToken cancellationToken)
    {
        var result = await commandService.Handle(new Domain.Model.Commands.RemovePartFromSubsystemCommand(hvofSystemId, subsystemId, partId), cancellationToken);
        return EquipmentActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory, _ => NoContent(), partId);
    }

    /// <summary>Adds a part to a subsystem.</summary>
    [HttpPost("{subsystemId:int}/parts")]
    [EndpointSummary("Add a part to a subsystem")]
    [ProducesResponseType(typeof(HvofPartResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddPart(int hvofSystemId, int subsystemId, [FromBody] AddPartResource resource, CancellationToken cancellationToken)
    {
        var command = AddPartCommandFromResourceAssembler.ToCommandFromResource(hvofSystemId, subsystemId, resource);
        var result = await commandService.Handle(command, cancellationToken);
        object detailArgument = result.Error switch
        {
            Domain.Model.EquipmentError.HvofSystemNotFound => hvofSystemId,
            Domain.Model.EquipmentError.SubsystemNotFound => subsystemId,
            _ => resource.SerialNumber
        };
        return EquipmentActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            part => Created($"/api/v1/hvof-systems/{hvofSystemId}/subsystems", HvofPartResourceFromEntityAssembler.ToResourceFromEntity(part)),
            detailArgument);
    }
}
