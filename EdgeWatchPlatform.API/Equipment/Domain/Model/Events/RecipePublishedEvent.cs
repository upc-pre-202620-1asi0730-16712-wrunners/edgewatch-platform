using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Equipment.Domain.Model.Events;

public record RecipePublishedEvent(int RecipeId, int HvofSystemId, int RecipeNumber, string Name) : IEvent;
