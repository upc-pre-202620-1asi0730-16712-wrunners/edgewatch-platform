using System.Net.Mime;
using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;
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
}
