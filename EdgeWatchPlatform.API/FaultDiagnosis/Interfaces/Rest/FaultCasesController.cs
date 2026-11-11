using System.Net.Mime;
using EdgeWatchPlatform.API.FaultDiagnosis.Application.CommandServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Application.QueryServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Errors;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Queries;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Transform;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using IamUser = EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates.User;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest;

/// <summary>Fault cases opened from out-of-band readings.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Fault Cases")]
public class FaultCasesController(
    IFaultCaseCommandService commandService,
    IFaultCaseQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Lists the cases of the organization, optionally filtered by session and status (open, acknowledged, resolved).</summary>
    [HttpGet]
    [EndpointSummary("Get the fault cases of the organization")]
    [ProducesResponseType(typeof(IEnumerable<FaultCaseResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFaultCases([FromQuery] int? spraySessionId, [FromQuery] string? status, CancellationToken cancellationToken)
    {
        var organizationId = ((IamUser)HttpContext.Items["User"]!).OrganizationId;
        FaultCaseStatus? statusFilter = status is not null && status.IsValidEnum<FaultCaseStatus>() ? status.ToEnum<FaultCaseStatus>() : null;
        var cases = await queryService.Handle(new GetAllFaultCasesByOrganizationIdQuery(organizationId, spraySessionId, statusFilter), cancellationToken);
        return Ok(cases.Select(FaultCaseResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets a case by id. Cases of other organizations are reported as not found.</summary>
    [HttpGet("{id:int}", Name = "GetFaultCaseById")]
    [EndpointSummary("Get a fault case by id")]
    [ProducesResponseType(typeof(FaultCaseResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetFaultCaseById(int id, CancellationToken cancellationToken)
    {
        var organizationId = ((IamUser)HttpContext.Items["User"]!).OrganizationId;
        var faultCase = await queryService.Handle(new GetFaultCaseByIdQuery(id), cancellationToken);
        return faultCase is null || faultCase.OrganizationId != organizationId
            ? FaultDiagnosisActionResultAssembler.ToActionResultFromError(this, FaultDiagnosisError.FaultCaseNotFound, FaultDiagnosisErrors.FaultCaseNotFound, problemDetailsFactory, id)
            : Ok(FaultCaseResourceFromEntityAssembler.ToResourceFromEntity(faultCase));
    }

    /// <summary>Acknowledges or resolves a case; the signed-in user is recorded.</summary>
    [HttpPatch("{id:int}")]
    [EndpointSummary("Acknowledge or resolve a fault case")]
    [ProducesResponseType(typeof(FaultCaseResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeFaultCaseStatus(int id, [FromBody] ChangeFaultCaseStatusResource resource, CancellationToken cancellationToken)
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        var existing = await queryService.Handle(new GetFaultCaseByIdQuery(id), cancellationToken);
        if (existing is null || existing.OrganizationId != user.OrganizationId)
            return FaultDiagnosisActionResultAssembler.ToActionResultFromError(this, FaultDiagnosisError.FaultCaseNotFound, FaultDiagnosisErrors.FaultCaseNotFound, problemDetailsFactory, id);
        var result = resource.Status == "acknowledged"
            ? await commandService.Handle(new AcknowledgeFaultCaseCommand(id, user.Id), cancellationToken)
            : await commandService.Handle(new ResolveFaultCaseCommand(id, user.Id, resource.ResolutionNotes), cancellationToken);
        return FaultDiagnosisActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            faultCase => Ok(FaultCaseResourceFromEntityAssembler.ToResourceFromEntity(faultCase)), id);
    }
}
