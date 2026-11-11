using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EdgeWatchPlatform.API.Shared.Application.Patterns;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Localization;
using EdgeWatchPlatform.API.Traceability.Application.Errors;
using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST;

/// <summary>Recuperation (work order) endpoints.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Recuperations")]
public class RecuperationsController(
    IRecuperationCommandService commandService,
    IRecuperationQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    /// <summary>Opens a recuperation in RECEIVED status and marks its component IN_RECUPERATION.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(RecuperationResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> OpenRecuperation([FromBody] OpenRecuperationResource resource, CancellationToken cancellationToken)
    {
        var command = OpenRecuperationCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromOpenRecuperationResultAssembler.ToActionResult(result, localizer, Request);
    }

    /// <summary>Lists recuperations, optionally filtered by supplier organization and/or customer.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<RecuperationResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllRecuperations(
        [FromQuery] [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int? supplierOrganizationId,
        [FromQuery] [Range(1, int.MaxValue, ErrorMessage = "InvalidCustomerId")] int? customerId,
        CancellationToken cancellationToken)
    {
        IEnumerable<Recuperation> recuperations = customerId is { } customer
            ? await queryService.Handle(new GetAllRecuperationsByCustomerIdQuery(customer), cancellationToken)
            : supplierOrganizationId is { } organization
                ? await queryService.Handle(new GetAllRecuperationsBySupplierOrganizationIdQuery(organization), cancellationToken)
                : await queryService.Handle(new GetAllRecuperationsQuery(), cancellationToken);

        if (customerId is not null && supplierOrganizationId is { } supplier)
            recuperations = recuperations.Where(r => r.SupplierOrganizationId == supplier);

        return Ok(recuperations.Select(RecuperationResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets a recuperation by id, including its linked sessions.</summary>
    [HttpGet("{id:int}", Name = "GetRecuperationById")]
    [ProducesResponseType(typeof(RecuperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecuperationById(int id, CancellationToken cancellationToken)
    {
        var recuperation = await queryService.Handle(new GetRecuperationByIdQuery(id), cancellationToken);
        return recuperation is null
            ? LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "RecuperationNotFound", Request.Path)
            : Ok(RecuperationResourceFromEntityAssembler.ToResourceFromEntity(recuperation));
    }

    /// <summary>Updates the editable data of a recuperation.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(RecuperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateRecuperation(int id, [FromBody] UpdateRecuperationResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateRecuperationCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromUpdateRecuperationResultAssembler.ToActionResult(result, localizer, Request);
    }

    /// <summary>
    ///     Changes the status of a recuperation. Allowed: RECEIVED→IN_PROGRESS, IN_PROGRESS→COMPLETED,
    ///     COMPLETED→DELIVERED|REWORK, REWORK→IN_PROGRESS. DELIVERED puts the component back IN_SERVICE.
    /// </summary>
    [HttpPatch("{id:int}")]
    [ProducesResponseType(typeof(RecuperationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> ChangeRecuperationStatus(int id, [FromBody] ChangeRecuperationStatusResource resource, CancellationToken cancellationToken)
    {
        var command = ChangeRecuperationStatusCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command, cancellationToken);

        var currentStatus = result is Result<Recuperation, ChangeRecuperationStatusError>.Failure { Error: ChangeRecuperationStatusError.InvalidTransition }
            ? (await queryService.Handle(new GetRecuperationByIdQuery(id), cancellationToken))?.Status
            : null;

        return ActionResultFromChangeRecuperationStatusResultAssembler.ToActionResult(result, currentStatus, command.Status, localizer, Request);
    }
}
