using System.Net.Mime;
using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST;

/// <summary>Controllers (PLC, OPC UA server, gateway) of an HVOF system.</summary>
[Authorize]
[ApiController]
[Route("api/v1/hvof-systems/{hvofSystemId:int}/controllers")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("HVOF Systems")]
public class HvofSystemControllersController(
    IHvofSystemCommandService commandService,
    IHvofSystemQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Lists the controllers of an HVOF system.</summary>
    [HttpGet]
    [EndpointSummary("Get the controllers of an HVOF system")]
    [ProducesResponseType(typeof(IEnumerable<ControllerResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllControllersByHvofSystemId(int hvofSystemId, CancellationToken cancellationToken)
    {
        var controllers = await queryService.Handle(new GetAllControllersByHvofSystemIdQuery(hvofSystemId), cancellationToken);
        return EquipmentActionResultAssembler.ToActionResultFromSystemCollection(this, controllers, problemDetailsFactory,
            items => Ok(items.Select(ControllerResourceFromEntityAssembler.ToResourceFromEntity)), hvofSystemId);
    }

    /// <summary>Adds a controller to an HVOF system.</summary>
    [HttpPost]
    [EndpointSummary("Add a controller to an HVOF system")]
    [ProducesResponseType(typeof(ControllerResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> AddController(int hvofSystemId, [FromBody] AddControllerResource resource, CancellationToken cancellationToken)
    {
        var command = AddControllerCommandFromResourceAssembler.ToCommandFromResource(hvofSystemId, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return EquipmentActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            controller => Created($"/api/v1/hvof-systems/{hvofSystemId}/controllers", ControllerResourceFromEntityAssembler.ToResourceFromEntity(controller)),
            result.Error is Domain.Model.EquipmentError.HvofSystemNotFound ? (object)hvofSystemId : resource.ControllerNumber);
    }
}
