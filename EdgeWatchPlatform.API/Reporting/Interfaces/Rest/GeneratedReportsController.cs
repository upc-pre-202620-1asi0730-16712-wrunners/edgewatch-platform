using System.Net.Mime;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Reporting.Application.CommandServices;
using EdgeWatchPlatform.API.Reporting.Application.QueryServices;
using EdgeWatchPlatform.API.Reporting.Domain.Model;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Errors;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Queries;
using EdgeWatchPlatform.API.Reporting.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Reporting.Interfaces.Rest.Transform;
using EdgeWatchPlatform.API.Shared.Infrastructure.Persistence.EntityFrameworkCore.Configuration.Extensions;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using IamUser = EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates.User;

namespace EdgeWatchPlatform.API.Reporting.Interfaces.Rest;

/// <summary>Registry of the reports rendered from templates by the signed-in user's organization.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Generated Reports")]
public class GeneratedReportsController(
    IGeneratedReportCommandService commandService,
    IGeneratedReportQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Lists the generated reports, optionally by subject (<c>SPRAY_SESSION</c> or <c>RECUPERATION</c> and its id).</summary>
    [HttpGet]
    [EndpointSummary("Get the generated reports of the organization")]
    [ProducesResponseType(typeof(IEnumerable<GeneratedReportResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllGeneratedReports([FromQuery] string? subjectType, [FromQuery] int? subjectId, CancellationToken cancellationToken)
    {
        var organizationId = ((IamUser)HttpContext.Items["User"]!).OrganizationId;
        ReportSubjectType? type = subjectType is not null && subjectType.IsValidEnum<ReportSubjectType>() ? subjectType.ToEnum<ReportSubjectType>() : null;
        var reports = await queryService.Handle(new GetAllGeneratedReportsByOrganizationIdQuery(organizationId, type, subjectId), cancellationToken);
        return Ok(reports.Select(GeneratedReportResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets a generated report by id. Reports of other organizations are reported as not found.</summary>
    [HttpGet("{id:int}", Name = "GetGeneratedReportById")]
    [EndpointSummary("Get a generated report by id")]
    [ProducesResponseType(typeof(GeneratedReportResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetGeneratedReportById(int id, CancellationToken cancellationToken)
    {
        var organizationId = ((IamUser)HttpContext.Items["User"]!).OrganizationId;
        var report = await queryService.Handle(new GetGeneratedReportByIdQuery(id), cancellationToken);
        return report is null || report.OrganizationId != organizationId
            ? ReportingActionResultAssembler.ToActionResultFromError(this, ReportingError.GeneratedReportNotFound, ReportingErrors.GeneratedReportNotFound, problemDetailsFactory, id)
            : Ok(GeneratedReportResourceFromEntityAssembler.ToResourceFromEntity(report));
    }

    /// <summary>Registers that a report was rendered from a template for a session or a recuperation.</summary>
    [HttpPost]
    [EndpointSummary("Register a generated report")]
    [ProducesResponseType(typeof(GeneratedReportResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> RegisterGeneratedReport([FromBody] RegisterGeneratedReportResource resource, CancellationToken cancellationToken)
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        var command = RegisterGeneratedReportCommandFromResourceAssembler.ToCommandFromResource(user.OrganizationId, user.Id, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ReportingActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            report => CreatedAtRoute("GetGeneratedReportById", new { id = report.Id }, GeneratedReportResourceFromEntityAssembler.ToResourceFromEntity(report)), resource.ReportTemplateId);
    }
}
