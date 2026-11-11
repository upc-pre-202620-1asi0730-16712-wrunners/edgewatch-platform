using System.Net.Mime;
using EdgeWatchPlatform.API.Iam.Application.QueryServices;
using EdgeWatchPlatform.API.Iam.Domain.Model.Queries;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Resources;
using EdgeWatchPlatform.API.Iam.Interfaces.Rest.Transform;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.Iam.Interfaces.Rest;

/// <summary>Role catalog endpoint.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Roles")]
public class RolesController(IRoleQueryService roleQueryService) : ControllerBase
{
    /// <summary>Lists every role of the platform.</summary>
    [HttpGet]
    [AllowAnonymous]
    [EndpointSummary("Get all roles")]
    [ProducesResponseType(typeof(IEnumerable<RoleResource>), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAllRoles(CancellationToken cancellationToken)
    {
        var roles = await roleQueryService.Handle(new GetAllRolesQuery(), cancellationToken);
        return Ok(roles.Select(RoleResourceFromEntityAssembler.ToResourceFromEntity));
    }
}
