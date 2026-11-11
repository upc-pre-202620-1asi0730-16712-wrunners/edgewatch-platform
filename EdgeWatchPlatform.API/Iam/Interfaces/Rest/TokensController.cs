using System.Net.Mime;
using EdgeWatchPlatform.API.Iam.Application.CommandServices;
using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model;
using EdgeWatchPlatform.API.Iam.Domain.Model.Errors;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest;

/// <summary>Authentication endpoint: exchanges credentials for a bearer token.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Tokens")]
public class TokensController(
    IUserCommandService userCommandService,
    IOrganizationQueryService organizationQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Signs in a user and issues a JWT bearer token.</summary>
    [HttpPost]
    [AllowAnonymous]
    [EndpointSummary("Sign in and create a bearer token")]
    [ProducesResponseType(typeof(AuthenticatedUserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> CreateToken([FromBody] SignInResource resource, CancellationToken cancellationToken)
    {
        var command = SignInCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        if (result.IsFailure) return IamActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory, _ => Ok());

        var (user, token) = result.Value;
        var organization = await organizationQueryService.Handle(new GetOrganizationByIdQuery(user.OrganizationId), cancellationToken);
        if (organization is null)
            return IamActionResultAssembler.ToActionResultFromError(this, IamError.OrganizationNotFound, IamErrors.OrganizationNotFound,
                problemDetailsFactory, user.OrganizationId);
        return Ok(AuthenticatedUserResourceFromEntityAssembler.ToResourceFromEntity(user, organization, token));
    }
}
