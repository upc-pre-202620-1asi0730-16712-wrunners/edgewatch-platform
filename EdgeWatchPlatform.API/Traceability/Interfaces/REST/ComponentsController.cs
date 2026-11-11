using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Localization;
using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST;

/// <summary>Recovered component endpoints.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Components")]
public class ComponentsController(
    IRecoveredComponentCommandService commandService,
    IRecoveredComponentQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    /// <summary>Registers a component of a customer. It starts in RECEIVED status.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ComponentResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterComponent([FromBody] RegisterComponentResource resource, CancellationToken cancellationToken)
    {
        var command = RegisterComponentCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromRegisterComponentResultAssembler.ToActionResult(result, localizer, Request);
    }

    /// <summary>Lists components, optionally filtered by customer.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ComponentResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllComponents([FromQuery] [Range(1, int.MaxValue, ErrorMessage = "InvalidCustomerId")] int? customerId, CancellationToken cancellationToken)
    {
        var components = customerId is { } id
            ? await queryService.Handle(new GetAllComponentsByCustomerIdQuery(id), cancellationToken)
            : await queryService.Handle(new GetAllComponentsQuery(), cancellationToken);
        return Ok(components.Select(ComponentResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets a component by id.</summary>
    [HttpGet("{id:int}", Name = "GetComponentById")]
    [ProducesResponseType(typeof(ComponentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetComponentById(int id, CancellationToken cancellationToken)
    {
        var component = await queryService.Handle(new GetComponentByIdQuery(id), cancellationToken);
        return component is null
            ? LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "ComponentNotFound", Request.Path)
            : Ok(ComponentResourceFromEntityAssembler.ToResourceFromEntity(component));
    }

    /// <summary>Updates the editable data and status of a component.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ComponentResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateComponent(int id, [FromBody] UpdateComponentResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateComponentCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromUpdateComponentResultAssembler.ToActionResult(result, localizer, Request);
    }
}
