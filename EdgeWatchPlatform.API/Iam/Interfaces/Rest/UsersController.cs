using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EdgeWatchPlatform.API.Iam.Application.CommandServices;
using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model;
using EdgeWatchPlatform.API.Iam.Domain.Model.Entities;
using EdgeWatchPlatform.API.Iam.Domain.Model.ValueObjects;
using EdgeWatchPlatform.API.Iam.Domain.Model.Errors;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;
using IamUser = EdgeWatchPlatform.API.Iam.Domain.Model.Aggregates.User;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest;

/// <summary>User endpoints: sign-up, profile and role assignment.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Users")]
public class UsersController(
    IUserCommandService userCommandService,
    IUserQueryService userQueryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Registers an organization and its administrator user (ROLE_ORG_ADMIN).</summary>
    [HttpPost]
    [AllowAnonymous]
    [EndpointSummary("Sign up an organization and its administrator")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> SignUp([FromBody] SignUpResource resource, CancellationToken cancellationToken)
    {
        var command = SignUpCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        var detailArgument = result.Error is IamError.RucAlreadyTaken ? resource.Ruc : resource.Email;
        return IamActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            user => CreatedAtRoute("GetUserById", new { id = user.Id }, UserResourceFromEntityAssembler.ToResourceFromEntity(user)),
            detailArgument);
    }

    /// <summary>Gets the signed-in user.</summary>
    [HttpGet("me")]
    [EndpointSummary("Get the signed-in user")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public IActionResult GetCurrentUser()
    {
        var user = (IamUser)HttpContext.Items["User"]!;
        return Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user));
    }

    /// <summary>Gets a user by id. Users of other organizations are reported as not found.</summary>
    [HttpGet("{id:int}", Name = "GetUserById")]
    [EndpointSummary("Get a user by id")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(int id, CancellationToken cancellationToken)
    {
        var currentUser = (IamUser)HttpContext.Items["User"]!;
        var user = await userQueryService.Handle(new GetUserByIdQuery(id), cancellationToken);
        if (user is not null && user.OrganizationId != currentUser.OrganizationId) user = null;
        return IamActionResultAssembler.ToActionResultFromGetUserResult(this, user, problemDetailsFactory,
            found => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(found)), id);
    }

    /// <summary>Lists the users of an organization (only the signed-in user's organization).</summary>
    [HttpGet]
    [EndpointSummary("Get the users of an organization")]
    [ProducesResponseType(typeof(IEnumerable<UserResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetAllUsersByOrganizationId([FromQuery] [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int organizationId, CancellationToken cancellationToken)
    {
        var currentUser = (IamUser)HttpContext.Items["User"]!;
        if (currentUser.OrganizationId != organizationId)
            return IamActionResultAssembler.ToActionResultFromError(this, IamError.Forbidden, IamErrors.Forbidden, problemDetailsFactory);
        var users = await userQueryService.Handle(new GetAllUsersByOrganizationIdQuery(organizationId), cancellationToken);
        return Ok(users.Select(UserResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Replaces the roles of a user of the signed-in administrator's organization.</summary>
    [HttpPut("{id:int}/roles")]
    [EndpointSummary("Assign roles to a user")]
    [ProducesResponseType(typeof(UserResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status403Forbidden)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> AssignRolesToUser(int id, [FromBody] AssignRolesToUserResource resource, CancellationToken cancellationToken)
    {
        var currentUser = (IamUser)HttpContext.Items["User"]!;
        if (!currentUser.HasRole(Role.ToIdFromName(Roles.RoleOrgAdmin)))
            return IamActionResultAssembler.ToActionResultFromError(this, IamError.Forbidden, IamErrors.Forbidden, problemDetailsFactory);
        var target = await userQueryService.Handle(new GetUserByIdQuery(id), cancellationToken);
        if (target is null || target.OrganizationId != currentUser.OrganizationId)
            return IamActionResultAssembler.ToActionResultFromError(this, IamError.UserNotFound, IamErrors.UserNotFound, problemDetailsFactory, id);
        var command = AssignRolesToUserCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await userCommandService.Handle(command, cancellationToken);
        return IamActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            user => Ok(UserResourceFromEntityAssembler.ToResourceFromEntity(user)), id);
    }
}
