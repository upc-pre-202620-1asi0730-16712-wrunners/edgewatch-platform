using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Errors;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Resources;
using EdgeWatchPlatform.API.Equipment.Interfaces.REST.Transform;
using EdgeWatchPlatform.API.Iam.Infrastructure.Pipeline.Middleware.Attributes;
using EdgeWatchPlatform.API.Shared.Interfaces.Rest.ProblemDetails;
using Microsoft.AspNetCore.Mvc;

namespace EdgeWatchPlatform.API.Equipment.Interfaces.REST;

/// <summary>Spray recipe endpoints.</summary>
[Authorize]
[ApiController]
[Route("api/v1/[controller]")]
[Produces(MediaTypeNames.Application.Json)]
[Tags("Recipes")]
public class RecipesController(
    IRecipeCommandService commandService,
    IRecipeQueryService queryService,
    ProblemDetailsFactory problemDetailsFactory) : ControllerBase
{
    /// <summary>Creates a DRAFT recipe for an HVOF system.</summary>
    [HttpPost]
    [EndpointSummary("Create a recipe")]
    [ProducesResponseType(typeof(RecipeResource), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> CreateRecipe([FromBody] CreateRecipeResource resource, CancellationToken cancellationToken)
    {
        CreateRecipeCommand command;
        try
        {
            command = CreateRecipeCommandFromResourceAssembler.ToCommandFromResource(resource);
        }
        catch (ArgumentException ex)
        {
            return EquipmentActionResultAssembler.ToActionResultFromError(this, EquipmentError.InvalidRecipeParameters,
                EquipmentErrors.InvalidRecipeParameters(ex.Message), problemDetailsFactory, ex.Message);
        }

        var result = await commandService.Handle(command, cancellationToken);
        object detailArgument = result.Error switch
        {
            EquipmentError.HvofSystemNotFound => resource.HvofSystemId,
            EquipmentError.InvalidRecipeParameters => result.DomainError.Message,
            _ => resource.RecipeNumber
        };
        return EquipmentActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            recipe => CreatedAtRoute("GetRecipeById", new { id = recipe.Id }, RecipeResourceFromEntityAssembler.ToResourceFromEntity(recipe)), detailArgument);
    }

    /// <summary>Gets a recipe by id with its parameters.</summary>
    [HttpGet("{id:int}", Name = "GetRecipeById")]
    [EndpointSummary("Get a recipe by id")]
    [ProducesResponseType(typeof(RecipeResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetRecipeById(int id, CancellationToken cancellationToken)
    {
        var recipe = await queryService.Handle(new GetRecipeByIdQuery(id), cancellationToken);
        return recipe is null
            ? EquipmentActionResultAssembler.ToActionResultFromError(this, EquipmentError.RecipeNotFound, EquipmentErrors.RecipeNotFound, problemDetailsFactory, id)
            : Ok(RecipeResourceFromEntityAssembler.ToResourceFromEntity(recipe));
    }

    /// <summary>Lists the recipes of an HVOF system.</summary>
    [HttpGet]
    [EndpointSummary("Get the recipes of an HVOF system")]
    [ProducesResponseType(typeof(IEnumerable<RecipeResource>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetAllRecipesByHvofSystemId([FromQuery] [Range(1, int.MaxValue, ErrorMessage = "InvalidHvofSystemId")] int hvofSystemId, CancellationToken cancellationToken)
    {
        var recipes = await queryService.Handle(new GetAllRecipesByHvofSystemIdQuery(hvofSystemId), cancellationToken);
        return Ok(recipes.Select(RecipeResourceFromEntityAssembler.ToResourceFromEntity));
    }

    /// <summary>Replaces the data of a DRAFT recipe.</summary>
    [HttpPut("{id:int}")]
    [EndpointSummary("Update a draft recipe")]
    [ProducesResponseType(typeof(RecipeResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> UpdateRecipe(int id, [FromBody] UpdateRecipeResource resource, CancellationToken cancellationToken)
    {
        UpdateRecipeCommand command;
        try
        {
            command = UpdateCommandsFromResourceAssembler.ToCommandFromResource(id, resource);
        }
        catch (ArgumentException ex)
        {
            return EquipmentActionResultAssembler.ToActionResultFromError(this, EquipmentError.InvalidRecipeParameters,
                EquipmentErrors.InvalidRecipeParameters(ex.Message), problemDetailsFactory, ex.Message);
        }

        var result = await commandService.Handle(command, cancellationToken);
        return EquipmentActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            recipe => Ok(RecipeResourceFromEntityAssembler.ToResourceFromEntity(recipe)),
            result.Error is EquipmentError.InvalidRecipeParameters ? result.DomainError.Message : id);
    }

    /// <summary>Changes the recipe status: ACTIVE publishes a draft (retiring the previous active one), RETIRED retires an active recipe.</summary>
    [HttpPatch("{id:int}/status")]
    [EndpointSummary("Publish or retire a recipe")]
    [ProducesResponseType(typeof(RecipeResource), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status422UnprocessableEntity)]
    public async Task<IActionResult> ChangeRecipeStatus(int id, [FromBody] ChangeRecipeStatusResource resource, CancellationToken cancellationToken)
    {
        var result = resource.Status == "ACTIVE"
            ? await commandService.Handle(new PublishRecipeCommand(id), cancellationToken)
            : await commandService.Handle(new RetireRecipeCommand(id), cancellationToken);
        return EquipmentActionResultAssembler.ToActionResultFromResult(this, result, problemDetailsFactory,
            recipe => Ok(RecipeResourceFromEntityAssembler.ToResourceFromEntity(recipe)), id);
    }
}
