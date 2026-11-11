namespace EdgeWatchPlatform.API.Equipment.Interfaces.Acl;

/// <summary>Anti-corruption layer of Equipment: what other bounded contexts may ask about systems and recipes.</summary>
public interface IEquipmentContextFacade
{
    /// <summary>Organization that owns the HVOF system, or 0 when it does not exist.</summary>
    Task<int> FetchHvofSystemOrganizationId(int hvofSystemId, CancellationToken cancellationToken = default);

    /// <summary>Id of the ACTIVE recipe with that number for the system, or 0 when there is none.</summary>
    Task<int> FetchActiveRecipeId(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default);

    /// <summary>Thresholds of the ACTIVE recipe with that number, empty when there is none.</summary>
    Task<IReadOnlyCollection<RecipeBandSnapshot>> FetchActiveRecipeBands(int hvofSystemId, int recipeNumber, CancellationToken cancellationToken = default);
}
