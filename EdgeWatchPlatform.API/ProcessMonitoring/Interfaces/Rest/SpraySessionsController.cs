using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.ProcessMonitoring.Application.CommandServices;
using EdgeWatchPlatform.API.ProcessMonitoring.Application.QueryServices;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Commands;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Errors;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.Queries;
using EdgeWatchPlatform.API.ProcessMonitoring.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest.Transform;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using IamUser = EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates.User;

namespace EdgeWatchPlatform.API.ProcessMonitoring.Interfaces.Rest;

/// <summary>Spray session endpoints: start, end, readings and deviations.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Spray Sessions")]
public class SpraySessionsController(
    ISpraySessionCommandService commandService,
    ISpraySessionQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Starts a session on an HVOF system for a recuperation with the active recipe of the given number. The operator is the signed-in user.</summary>
    [HttpPost]
    [EndpointSummary("Start a spray session")]
    [ProducesResponseType(typeof(SpraySessionResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> StartSpraySession([FromBody] StartSpraySessionResource resource, CancellationToken cancellationToken)
    {
        var operatorId = ((IamUser)HttpContext.Items["User"]!).Id;
        var command = StartSpraySessionCommandFromResourceAssembler.ToCommandFromResource(resource, operatorId);
        var result = await commandService.Handle(command, cancellationToken);
        object detailArgument = result.Error switch
        {
            ProcessMonitoringError.HvofSystemNotFound => resource.HvofSystemId,
            ProcessMonitoringError.ActiveRecipeNotFound => resource.RecipeNumber,
            _ => resource.RecuperationId
        };
        return ProcessMonitoringActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            session => CreatedAtRoute("GetSpraySessionById", new { id = session.Id }, SpraySessionResourceFromEntityAssembler.ToResourceFromEntity(session)),
            detailArgument);
    }

    /// <summary>Gets a session by id.</summary>
    [HttpGet("{id:int}", Name = "GetSpraySessionById")]
    [EndpointSummary("Get a spray session by id")]
    [ProducesResponseType(typeof(SpraySessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetSpraySessionById(int id, CancellationToken cancellationToken)
    {
        var session = await queryService.Handle(new GetSpraySessionByIdQuery(id), cancellationToken);
        return session is null
            ? ProcessMonitoringActionResultAssembler.ToActionResultFromError(this, ProcessMonitoringError.SpraySessionNotFound, ProcessMonitoringErrors.SpraySessionNotFound, problemDetailsFactory, id)
            : Ok(SpraySessionResourceFromEntityAssembler.ToResourceFromEntity(session));
    }

    /// <summary>Lists the sessions of the signed-in user's organization, optionally filtered by recuperation.</summary>
    [HttpGet]
    [EndpointSummary("Get the spray sessions of the organization")]
    [ProducesResponseType(typeof(IEnumerable<SpraySessionResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllSpraySessions([FromQuery] [Range(1, int.MaxValue, ErrorMessage = "InvalidRecuperationId")] int? recuperationId, CancellationToken cancellationToken)
    {
        var organizationId = ((IamUser)HttpContext.Items["User"]!).OrganizationId;
        var sessions = recuperationId is { } recuperation
            ? (await queryService.Handle(new GetAllSpraySessionsByRecuperationIdQuery(recuperation), cancellationToken)).Where(s => s.OrganizationId == organizationId)
            : await queryService.Handle(new GetAllSpraySessionsByOrganizationIdQuery(organizationId), cancellationToken);
        return Ok(sessions.Select(SpraySessionResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Ends a session: <c>completed</c> or <c>aborted</c> (with an abort reason).</summary>
    [HttpPatch("{id:int}")]
    [EndpointSummary("Complete or abort a spray session")]
    [ProducesResponseType(typeof(SpraySessionResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeSpraySessionStatus(int id, [FromBody] ChangeSpraySessionStatusResource resource, CancellationToken cancellationToken)
    {
        var result = resource.Status == "completed"
            ? await commandService.Handle(new CompleteSpraySessionCommand(id), cancellationToken)
            : await commandService.Handle(new AbortSpraySessionCommand(id, (resource.AbortReason ?? "OTHER").ToEnum<AbortReason>()), cancellationToken);
        return ProcessMonitoringActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            session => Ok(SpraySessionResourceFromEntityAssembler.ToResourceFromEntity(session)), id);
    }

    /// <summary>Stores a batch of readings of an active session, classified against the frozen recipe bands.</summary>
    [HttpPost("{id:int}/readings")]
    [EndpointSummary("Ingest process readings")]
    [ProducesResponseType(typeof(IEnumerable<ProcessReadingResource>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> IngestProcessReadings(int id, [FromBody] [MinLength(1, ErrorMessage = "RequiredReadings")] IReadOnlyCollection<ProcessReadingItemResource> resources, CancellationToken cancellationToken)
    {
        var command = IngestProcessReadingsCommandFromResourceAssembler.ToCommandFromResource(id, resources);
        var result = await commandService.Handle(command, cancellationToken);
        return ProcessMonitoringActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            readings => Created($"/api/v1/spray-sessions/{id}/readings", readings.Select(ProcessReadingResourceFromEntityAssembler.ToResourceFromEntity)),
            result.Error is ProcessMonitoringError.InvalidReadings ? result.DomainError.Message : id);
    }

    /// <summary>Lists the readings of a session in time order, optionally filtered by band (nominal, out_of_nominal, warning, shutdown).</summary>
    [HttpGet("{id:int}/readings")]
    [EndpointSummary("Get the readings of a spray session")]
    [ProducesResponseType(typeof(IEnumerable<ProcessReadingResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReadingsBySpraySessionId(int id, [FromQuery] string? band, CancellationToken cancellationToken)
    {
        if (await queryService.Handle(new GetSpraySessionByIdQuery(id), cancellationToken) is null)
            return ProcessMonitoringActionResultAssembler.ToActionResultFromError(this, ProcessMonitoringError.SpraySessionNotFound, ProcessMonitoringErrors.SpraySessionNotFound, problemDetailsFactory, id);
        Band? bandFilter = band is not null && band.IsValidEnum<Band>() ? band.ToEnum<Band>() : null;
        var readings = await queryService.Handle(new GetAllReadingsBySpraySessionIdQuery(id, bandFilter), cancellationToken);
        return Ok(readings.Select(ProcessReadingResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Counts the warning and shutdown readings of a session.</summary>
    [HttpGet("{id:int}/deviations")]
    [EndpointSummary("Get the deviation count of a spray session")]
    [ProducesResponseType(typeof(DeviationCountResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDeviationsBySpraySessionId(int id, CancellationToken cancellationToken)
    {
        if (await queryService.Handle(new GetSpraySessionByIdQuery(id), cancellationToken) is null)
            return ProcessMonitoringActionResultAssembler.ToActionResultFromError(this, ProcessMonitoringError.SpraySessionNotFound, ProcessMonitoringErrors.SpraySessionNotFound, problemDetailsFactory, id);
        var (warning, shutdown) = await queryService.Handle(new GetDeviationCountBySpraySessionIdQuery(id), cancellationToken);
        return Ok(new DeviationCountResource(id, warning, shutdown, warning + shutdown));
    }
}
