using System.Net.Mime;
using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Reporting.Application.CommandServices;
using EdgeWatchPlatform.API.Reporting.Application.QueryServices;
using EdgeWatchPlatform.API.Reporting.Domain.Model;
using EdgeWatchPlatform.API.Reporting.Domain.Model.Commands;
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

/// <summary>Customizable report templates of the signed-in user's organization.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Report Templates")]
public class ReportTemplatesController(
    IReportTemplateCommandService commandService,
    IReportTemplateQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    private static bool CanManage(IamUser user) =>
        user.HasRole(Role.ToIdFromName(Roles.RoleOrgAdmin)) || user.HasRole(Role.ToIdFromName(Roles.RoleQualityEngineer));

    /// <summary>Lists the templates of the organization, optionally by kind; the standard ones are seeded the first time.</summary>
    [HttpGet]
    [EndpointSummary("Get the report templates of the organization")]
    [ProducesResponseType(typeof(IEnumerable<ReportTemplateResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllReportTemplates([FromQuery] string? reportKind, CancellationToken cancellationToken)
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        await commandService.Handle(new SeedDefaultReportTemplatesCommand(user.OrganizationId, user.Id), cancellationToken);
        ReportKind? kind = reportKind is not null && reportKind.IsValidEnum<ReportKind>() ? reportKind.ToEnum<ReportKind>() : null;
        var templates = await queryService.Handle(new GetAllReportTemplatesByOrganizationIdQuery(user.OrganizationId, kind), cancellationToken);
        return Ok(templates.Select(ReportTemplateResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets a template with its sections. Templates of other organizations are reported as not found.</summary>
    [HttpGet("{id:int}", Name = "GetReportTemplateById")]
    [EndpointSummary("Get a report template by id")]
    [ProducesResponseType(typeof(ReportTemplateResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetReportTemplateById(int id, CancellationToken cancellationToken)
    {
        var organizationId = ((IamUser)HttpContext.Items["User"]!).OrganizationId;
        var template = await queryService.Handle(new GetReportTemplateByIdQuery(id), cancellationToken);
        return template is null || template.OrganizationId != organizationId
            ? ReportingActionResultAssembler.ToActionResultFromError(this, ReportingError.ReportTemplateNotFound, ReportingErrors.ReportTemplateNotFound, problemDetailsFactory, id)
            : Ok(ReportTemplateResourceFromEntityAssembler.ToResourceFromEntity(template));
    }

    /// <summary>Creates a template (administrators and quality engineers).</summary>
    [HttpPost]
    [EndpointSummary("Create a report template")]
    [ProducesResponseType(typeof(ReportTemplateResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateReportTemplate([FromBody] CreateReportTemplateResource resource, CancellationToken cancellationToken)
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        if (!CanManage(user))
            return ReportingActionResultAssembler.ToActionResultFromError(this, ReportingError.Forbidden, ReportingErrors.Forbidden, problemDetailsFactory);
        var command = ReportTemplateCommandsFromResourceAssembler.ToCommandFromResource(user.OrganizationId, user.Id, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ReportingActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            template => CreatedAtRoute("GetReportTemplateById", new { id = template.Id }, ReportTemplateResourceFromEntityAssembler.ToResourceFromEntity(template)), resource.Name);
    }

    /// <summary>Replaces the layout of a template (administrators and quality engineers).</summary>
    [HttpPut("{id:int}")]
    [EndpointSummary("Update a report template")]
    [ProducesResponseType(typeof(ReportTemplateResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateReportTemplate(int id, [FromBody] UpdateReportTemplateResource resource, CancellationToken cancellationToken)
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        if (!CanManage(user))
            return ReportingActionResultAssembler.ToActionResultFromError(this, ReportingError.Forbidden, ReportingErrors.Forbidden, problemDetailsFactory);
        var existing = await queryService.Handle(new GetReportTemplateByIdQuery(id), cancellationToken);
        if (existing is null || existing.OrganizationId != user.OrganizationId)
            return ReportingActionResultAssembler.ToActionResultFromError(this, ReportingError.ReportTemplateNotFound, ReportingErrors.ReportTemplateNotFound, problemDetailsFactory, id);
        var result = await commandService.Handle(ReportTemplateCommandsFromResourceAssembler.ToCommandFromResource(id, resource), cancellationToken);
        return ReportingActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            template => Ok(ReportTemplateResourceFromEntityAssembler.ToResourceFromEntity(template)), id);
    }

    /// <summary>Archives or reactivates a template (administrators and quality engineers).</summary>
    [HttpPatch("{id:int}/status")]
    [EndpointSummary("Archive or reactivate a report template")]
    [ProducesResponseType(typeof(ReportTemplateResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeReportTemplateStatus(int id, [FromBody] ChangeReportTemplateStatusResource resource, CancellationToken cancellationToken)
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        if (!CanManage(user))
            return ReportingActionResultAssembler.ToActionResultFromError(this, ReportingError.Forbidden, ReportingErrors.Forbidden, problemDetailsFactory);
        var existing = await queryService.Handle(new GetReportTemplateByIdQuery(id), cancellationToken);
        if (existing is null || existing.OrganizationId != user.OrganizationId)
            return ReportingActionResultAssembler.ToActionResultFromError(this, ReportingError.ReportTemplateNotFound, ReportingErrors.ReportTemplateNotFound, problemDetailsFactory, id);
        var result = await commandService.Handle(new ChangeReportTemplateStatusCommand(id, resource.Status.ToEnum<TemplateStatus>()), cancellationToken);
        return ReportingActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            template => Ok(ReportTemplateResourceFromEntityAssembler.ToResourceFromEntity(template)), id);
    }
}
