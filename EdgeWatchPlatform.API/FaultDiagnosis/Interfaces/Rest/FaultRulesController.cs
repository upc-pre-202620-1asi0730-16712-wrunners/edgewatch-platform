using System.Net.Mime;
using EdgeWatchPlatform.API.FaultDiagnosis.Application.CommandServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Application.QueryServices;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Commands;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Errors;
using EdgeWatchPlatform.API.FaultDiagnosis.Domain.Model.Queries;
using EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest.Transform;
using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using IamUser = EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates.User;

namespace EdgeWatchPlatform.API.FaultDiagnosis.Interfaces.Rest;

/// <summary>Cause-effect rules of the signed-in user's organization.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Fault Rules")]
public class FaultRulesController(
    IFaultRuleCommandService commandService,
    IFaultRuleQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Lists the rules of the organization; the defaults are seeded the first time.</summary>
    [HttpGet]
    [EndpointSummary("Get the fault rules of the organization")]
    [ProducesResponseType(typeof(IEnumerable<FaultRuleResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllFaultRules(CancellationToken cancellationToken)
    {
        var organizationId = ((IamUser)HttpContext.Items["User"]!).OrganizationId;
        await commandService.Handle(new SeedDefaultFaultRulesCommand(organizationId), cancellationToken);
        var rules = await queryService.Handle(new GetAllFaultRulesByOrganizationIdQuery(organizationId), cancellationToken);
        return Ok(rules.Select(FaultRuleResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Creates a rule (administrators only).</summary>
    [HttpPost]
    [EndpointSummary("Create a fault rule")]
    [ProducesResponseType(typeof(FaultRuleResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> CreateFaultRule([FromBody] CreateFaultRuleResource resource, CancellationToken cancellationToken)
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        if (!user.HasRole(Role.ToIdFromName(Roles.RoleOrgAdmin)))
            return FaultDiagnosisActionResultAssembler.ToActionResultFromError(this, FaultDiagnosisError.Forbidden, FaultDiagnosisErrors.Forbidden, problemDetailsFactory);
        var command = CreateFaultRuleCommandFromResourceAssembler.ToCommandFromResource(user.OrganizationId, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return FaultDiagnosisActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            rule => Created($"/api/v1/fault-rules/{rule.Id}", FaultRuleResourceFromEntityAssembler.ToResourceFromEntity(rule)), $"{resource.Parameter}/{resource.Band}");
    }

    /// <summary>Updates the cause, action and cooldown of a rule (administrators only).</summary>
    [HttpPut("{id:int}")]
    [EndpointSummary("Update a fault rule")]
    [ProducesResponseType(typeof(FaultRuleResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateFaultRule(int id, [FromBody] UpdateFaultRuleResource resource, CancellationToken cancellationToken)
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        if (!user.HasRole(Role.ToIdFromName(Roles.RoleOrgAdmin)))
            return FaultDiagnosisActionResultAssembler.ToActionResultFromError(this, FaultDiagnosisError.Forbidden, FaultDiagnosisErrors.Forbidden, problemDetailsFactory);
        var rules = await queryService.Handle(new GetAllFaultRulesByOrganizationIdQuery(user.OrganizationId), cancellationToken);
        if (rules.All(r => r.Id != id))
            return FaultDiagnosisActionResultAssembler.ToActionResultFromError(this, FaultDiagnosisError.FaultRuleNotFound, FaultDiagnosisErrors.FaultRuleNotFound, problemDetailsFactory, id);
        var result = await commandService.Handle(new UpdateFaultRuleCommand(id, resource.ProbableCause, resource.RecommendedAction, resource.CooldownSeconds), cancellationToken);
        return FaultDiagnosisActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            rule => Ok(FaultRuleResourceFromEntityAssembler.ToResourceFromEntity(rule)), id);
    }
}
