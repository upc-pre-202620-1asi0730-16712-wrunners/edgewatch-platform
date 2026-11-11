using EdgeWatchPlatform.API.Shared.Domain.Model.Events;

namespace EdgeWatchPlatform.API.Shared.Domain.Model.Aggregates.Internal;

/// <summary>Base class for aggregate roots: keeps the list of domain events raised by the aggregate.</summary>
public abstract class AggregateRoot : IAggregateRoot
{
    private readonly List<IEvent> _domainEvents = [];

    /// <inheritdoc />
    public IReadOnlyCollection<IEvent> DomainEvents => _domainEvents.AsReadOnly();

    /// <summary>Records a domain event to be dispatched when changes are committed.</summary>
    protected void AddDomainEvent(IEvent domainEvent) => _domainEvents.Add(domainEvent);

    /// <inheritdoc />
    public void ClearDomainEvents() => _domainEvents.Clear();
}
