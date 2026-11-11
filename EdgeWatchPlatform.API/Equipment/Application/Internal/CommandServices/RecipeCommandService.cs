using EdgeWatchPlatform.API.Equipment.Application.Services;
using EdgeWatchPlatform.API.Equipment.Domain.Model;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Commands;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Errors;
using EdgeWatchPlatform.API.Equipment.Domain.Repositories;
using EdgeWatchPlatform.API.Shared.Application.Model;
using EdgeWatchPlatform.API.Shared.Domain.Repositories;

namespace EdgeWatchPlatform.API.Equipment.Application.Internal.CommandServices;

public class RecipeCommandService(
    IRecipeRepository recipeRepository,
    IHvofSystemRepository hvofSystemRepository,
    IUnitOfWork unitOfWork,
    ILogger<RecipeCommandService> logger) : IRecipeCommandService
{
    public async Task<Result<Recipe>> Handle(CreateRecipeCommand command, CancellationToken cancellationToken = default)
    {
        if (await hvofSystemRepository.FindByIdAsync(command.HvofSystemId, cancellationToken) is null)
            return Result<Recipe>.Failure(EquipmentErrors.HvofSystemNotFound, EquipmentError.HvofSystemNotFound);
        if (await recipeRepository.ExistsDraftByHvofSystemIdAndNumberAsync(command.HvofSystemId, command.RecipeNumber, cancellationToken))
            return Result<Recipe>.Failure(EquipmentErrors.DuplicateRecipeNumber, EquipmentError.DuplicateRecipeNumber);

        try
        {
            var recipe = new Recipe(command);
            await recipeRepository.AddAsync(recipe, cancellationToken);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Recipe>.Success(recipe);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Invalid recipe {RecipeNumber} for HVOF system {HvofSystemId}", command.RecipeNumber, command.HvofSystemId);
            return Result<Recipe>.Failure(EquipmentErrors.InvalidRecipeParameters(ex.Message), EquipmentError.InvalidRecipeParameters);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error creating recipe {RecipeNumber} for HVOF system {HvofSystemId}", command.RecipeNumber, command.HvofSystemId);
            return Result<Recipe>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    /// <summary>Publishes a draft and retires the previously active recipe with the same number, in one unit of work.</summary>
    public async Task<Result<Recipe>> Handle(PublishRecipeCommand command, CancellationToken cancellationToken = default)
    {
        var recipe = await recipeRepository.FindByIdWithParametersAsync(command.RecipeId, cancellationToken);
        if (recipe is null) return Result<Recipe>.Failure(EquipmentErrors.RecipeNotFound, EquipmentError.RecipeNotFound);
        if (!recipe.TryPublish()) return Result<Recipe>.Failure(EquipmentErrors.InvalidRecipeTransition, EquipmentError.InvalidRecipeTransition);

        try
        {
            var previous = await recipeRepository.FindActiveByHvofSystemIdAndNumberAsync(recipe.HvofSystemId, recipe.RecipeNumber, cancellationToken);
            if (previous is not null && previous.Id != recipe.Id)
            {
                previous.TryRetire();
                recipeRepository.Update(previous);
            }
            recipeRepository.Update(recipe);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Recipe>.Success(recipe);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error publishing recipe {RecipeId}", command.RecipeId);
            return Result<Recipe>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }

    public async Task<Result<Recipe>> Handle(RetireRecipeCommand command, CancellationToken cancellationToken = default)
    {
        var recipe = await recipeRepository.FindByIdWithParametersAsync(command.RecipeId, cancellationToken);
        if (recipe is null) return Result<Recipe>.Failure(EquipmentErrors.RecipeNotFound, EquipmentError.RecipeNotFound);
        if (!recipe.TryRetire()) return Result<Recipe>.Failure(EquipmentErrors.InvalidRecipeTransition, EquipmentError.InvalidRecipeTransition);

        try
        {
            recipeRepository.Update(recipe);
            await unitOfWork.CompleteAsync(cancellationToken);
            return Result<Recipe>.Success(recipe);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unexpected error retiring recipe {RecipeId}", command.RecipeId);
            return Result<Recipe>.Failure(EquipmentErrors.InternalServerError, EquipmentError.InternalServerError);
        }
    }
}
