using EdgeWatchPlatform.API.Equipment.Domain.Model.Aggregates;
using EdgeWatchPlatform.API.Equipment.Domain.Model.Queries;

namespace EdgeWatchPlatform.API.Equipment.Application.Services;

public interface IRecipeQueryService
{
    Task<Recipe?> Handle(GetRecipeByIdQuery query, CancellationToken cancellationToken = default);
    Task<IEnumerable<Recipe>> Handle(GetAllRecipesByHvofSystemIdQuery query, CancellationToken cancellationToken = default);
    Task<Recipe?> Handle(GetActiveRecipeByHvofSystemIdAndNumberQuery query, CancellationToken cancellationToken = default);
}
