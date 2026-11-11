namespace EdgeWatchPlatform.API.Traceability.Interfaces.Acl;

/// <summary>Anti-corruption layer of Traceability: what other bounded contexts may ask or request about recuperations.</summary>
public interface ITraceabilityContextFacade
{
    /// <summary>Supplier organization of a recuperation, or 0 when it does not exist.</summary>
    Task<int> FetchRecuperationSupplierOrganizationId(int recuperationId, CancellationToken cancellationToken = default);

    /// <summary>Links a monitoring session to a recuperation (RECEIVED moves to IN_PROGRESS). Returns false when the recuperation does not exist.</summary>
    Task<bool> LinkSessionToRecuperation(int recuperationId, int sessionId, CancellationToken cancellationToken = default);
}
