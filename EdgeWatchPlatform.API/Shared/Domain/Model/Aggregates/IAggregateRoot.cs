using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates;

/// <summary>Contract of an aggregate root able to record domain events.</summary>
public interface IAggregateRoot
{
    /// <summary>Domain events recorded by the aggregate and not yet dispatched.</summary>
    IReadOnlyCollection<IEvent> DomainEvents { get; }

    /// <summary>Removes every recorded domain event.</summary>
    void ClearDomainEvents();
}
