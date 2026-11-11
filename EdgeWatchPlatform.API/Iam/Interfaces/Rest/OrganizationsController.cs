using System.Net.Mime;
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

/// <summary>Organization endpoints.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Organizations")]
public class OrganizationsController(IOrganizationQueryService organizationQueryService, ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Gets an organization by id.</summary>
    [HttpGet("{id:int}", Name = "GetOrganizationById")]
    [EndpointSummary("Get an organization by id")]
    [ProducesResponseType(typeof(OrganizationResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetOrganizationById(int id, CancellationToken cancellationToken)
    {
        var organization = await organizationQueryService.Handle(new GetOrganizationByIdQuery(id), cancellationToken);
        return organization is null
            ? IamActionResultAssembler.ToActionResultFromError(this, IamError.OrganizationNotFound, IamErrors.OrganizationNotFound, problemDetailsFactory, id)
            : Ok(OrganizationResourceFromEntityAssembler.ToResourceFromEntity(organization));
    }
}
