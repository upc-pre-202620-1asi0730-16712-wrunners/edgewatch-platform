using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST;

/// <summary>HVOF system endpoints.</summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("HVOF Systems")]
public class HvofSystemsController(
    IHvofSystemCommandService commandService,
    IHvofSystemQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    /// <summary>Registers an HVOF system for an organization.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(HvofSystemResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterHvofSystem([FromBody] RegisterHvofSystemResource resource, CancellationToken cancellationToken)
    {
        var command = RegisterHvofSystemCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromRegisterHvofSystemResultAssembler.ToActionResult(result, localizer, Request);
    }

    /// <summary>Gets an HVOF system by id.</summary>
    [HttpGet("{id:int}", Name = "GetHvofSystemById")]
    [ProducesResponseType(typeof(HvofSystemResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetHvofSystemById(int id, CancellationToken cancellationToken)
    {
        var system = await queryService.Handle(new GetHvofSystemByIdQuery(id), cancellationToken);
        return system is null
            ? LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "HvofSystemNotFound", Request.Path)
            : Ok(HvofSystemResourceFromEntityAssembler.ToResourceFromEntity(system));
    }

    /// <summary>Lists the HVOF systems of an organization.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<HvofSystemResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllHvofSystemsByOrganizationId([FromQuery] [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int organizationId, CancellationToken cancellationToken)
    {
        var systems = await queryService.Handle(new GetAllHvofSystemsByOrganizationIdQuery(organizationId), cancellationToken);
        return Ok(systems.Select(HvofSystemResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
