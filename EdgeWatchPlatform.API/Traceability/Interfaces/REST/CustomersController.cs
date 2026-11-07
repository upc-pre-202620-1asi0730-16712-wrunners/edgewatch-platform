using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Configuration;
using EdgeWatchPlatform.API.Shared.Interfaces.ASP.Localization;
using EdgeWatchPlatform.API.Traceability.Application.Services;
using EdgeWatchPlatform.API.Traceability.Domain.Model.Queries;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Traceability.Interfaces.REST.Transform;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace EdgeWatchPlatform.API.Traceability.Interfaces.REST;

/// <summary>Customer endpoints.</summary>
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Customers")]
public class CustomersController(
    ICustomerCommandService commandService,
    ICustomerQueryService queryService,
    IStringLocalizer<SharedResource> localizer) : ControllerBase
{
    /// <summary>Registers a customer for a supplier organization.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(CustomerResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    public async Task<IActionResult> RegisterCustomer([FromBody] RegisterCustomerResource resource, CancellationToken cancellationToken)
    {
        var command = RegisterCustomerCommandFromResourceAssembler.ToCommandFromResource(resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromRegisterCustomerResultAssembler.ToActionResult(result, localizer, Request);
    }

    /// <summary>Lists customers, optionally filtered by supplier organization.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<CustomerResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllCustomers([FromQuery] [Range(1, int.MaxValue, ErrorMessage = "InvalidOrganizationId")] int? supplierOrganizationId, CancellationToken cancellationToken)
    {
        var customers = supplierOrganizationId is { } organizationId
            ? await queryService.Handle(new GetAllCustomersBySupplierOrganizationIdQuery(organizationId), cancellationToken)
            : await queryService.Handle(new GetAllCustomersQuery(), cancellationToken);
        return Ok(customers.Select(CustomerResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Gets a customer by id.</summary>
    [HttpGet("{id:int}", Name = "GetCustomerById")]
    [ProducesResponseType(typeof(CustomerResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetCustomerById(int id, CancellationToken cancellationToken)
    {
        var customer = await queryService.Handle(new GetCustomerByIdQuery(id), cancellationToken);
        return customer is null
            ? LocalizedProblemDetails.Create(localizer, StatusCodes.Status404NotFound, "NotFoundTitle", "CustomerNotFound", Request.Path)
            : Ok(CustomerResourceFromEntityAssembler.ToResourceFromEntity(customer));
    }

    /// <summary>Updates the editable data of a customer (the RUC is not editable).</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(CustomerResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateCustomer(int id, [FromBody] UpdateCustomerResource resource, CancellationToken cancellationToken)
    {
        var command = UpdateCustomerCommandFromResourceAssembler.ToCommandFromResource(id, resource);
        var result = await commandService.Handle(command, cancellationToken);
        return ActionResultFromUpdateCustomerResultAssembler.ToActionResult(result, localizer, Request);
    }
}
